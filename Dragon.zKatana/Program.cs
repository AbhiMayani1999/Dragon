using Dragon.Business.Process.Configs;
using Dragon.Business.Process.Queries;
using Dragon.Data;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dragon.zKatana
{
    internal class Program
    {
        private static async Task Main()
        {
            await GeneratePage(new DefaultContext(ConnectionProvider.Provider.GetConnection(ConfigProvider.MasterTenantName)), "LalludiPage", new Navigation());
            JobProcess jobProcess = new() { CurrentUser = new() { TenantCode = ConfigProvider.MasterTenantName } };
            await jobProcess.Pages();
            Console.ReadLine();
        }
        private struct PropertyDeclaration
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }
        private static async Task<bool> GeneratePage<T>(DefaultContext defaultContext, string name, T data)
        {
            using IDbContextTransaction generationTransection = defaultContext.Database.BeginTransaction();
            List<ComponentStructure> componentStructures = ComponentQueries.GetAllComponentStructure(defaultContext).ToList();

            Structure pageStructure = GeneratePageStructure(componentStructures, name);
            await defaultContext.Structure.AddAsync(pageStructure);

            Structure tableStructure = GenerateTableStructure(componentStructures, name);
            await defaultContext.Structure.AddAsync(tableStructure);

            Structure formStructure = GenerateFormStructure(componentStructures, name);
            await defaultContext.Structure.AddAsync(formStructure);
            await defaultContext.SaveChangesAsync();

            defaultContext.StructureSubStructure.Add(new() { Id = 0, ChildStructureId = tableStructure.Id, ParentStructureId = pageStructure.Id });
            defaultContext.StructurePropertyToStructure.Add(new() { Id = 0, ChildStructureId = tableStructure.Id, StructurePropertyId = StructurePropertyId(componentStructures, pageStructure, "Widgets") });

            defaultContext.StructureSubStructure.Add(new() { Id = 0, ChildStructureId = formStructure.Id, ParentStructureId = tableStructure.Id });
            defaultContext.StructurePropertyToStructure.Add(new() { Id = 0, ChildStructureId = formStructure.Id, StructurePropertyId = StructurePropertyId(componentStructures, tableStructure, "Form") });
            await defaultContext.SaveChangesAsync();

            List<PropertyDeclaration> propertyDeclarations = typeof(T).GetProperties().ToList().Select(d => new PropertyDeclaration() { Name = d.Name, Type = d.PropertyType.Name, Value = Convert.ToString(d.GetValue(data)) }).ToList();
            foreach (PropertyDeclaration property in propertyDeclarations)
            {
                Structure columnStructure = null;
                Structure formfieldStructure = null;
                if (property.Type == nameof(Boolean))
                {
                    columnStructure = GenerateColumnStructure(componentStructures, property, "BoolColumn");
                    formfieldStructure = GenerateFormFieldStructure(componentStructures, property, "CheckboxField");
                }
                else if (property.Type == nameof(Int32))
                {
                    columnStructure = GenerateColumnStructure(componentStructures, property, "IdColumn");
                    formfieldStructure = GenerateFormFieldStructure(componentStructures, property, "NumberField");
                }
                else if (property.Type == nameof(String))
                {
                    columnStructure = GenerateColumnStructure(componentStructures, property, "TextColumn");
                    formfieldStructure = GenerateFormFieldStructure(componentStructures, property, "TextField");
                }

                if (columnStructure != null)
                {
                    await defaultContext.Structure.AddAsync(columnStructure);
                    await defaultContext.SaveChangesAsync();
                    defaultContext.StructureSubStructure.Add(new() { Id = 0, ChildStructureId = columnStructure.Id, ParentStructureId = tableStructure.Id });
                    defaultContext.StructurePropertyToStructure.Add(new() { Id = 0, ChildStructureId = columnStructure.Id, StructurePropertyId = StructurePropertyId(componentStructures, tableStructure, "Fields") });
                    await defaultContext.SaveChangesAsync();
                }

                if (formfieldStructure != null)
                {
                    await defaultContext.Structure.AddAsync(formfieldStructure);
                    await defaultContext.SaveChangesAsync();
                    defaultContext.StructureSubStructure.Add(new() { Id = 0, ChildStructureId = formfieldStructure.Id, ParentStructureId = formStructure.Id });
                    defaultContext.StructurePropertyToStructure.Add(new() { Id = 0, ChildStructureId = formfieldStructure.Id, StructurePropertyId = StructurePropertyId(componentStructures, formStructure, "Fields") });
                    await defaultContext.SaveChangesAsync();
                }
            }

            generationTransection.Commit();
            return true;
        }

        private static Structure GeneratePageStructure(List<ComponentStructure> componentStructures, string name)
        {
            return GenerateComponentStructure(componentStructures, "Page", [
                new (){ Name = "Widgets", Type = "Component" },
                new (){ Name = "Title", Value = name.ToTitleCase() },
                new (){ Name = "State", Value = name.ToLowerCase() }
            ]);
        }

        #region [Table]
        private static Structure GenerateColumnStructure(List<ComponentStructure> componentStructures, PropertyDeclaration declaration, string structureType)
        {
            return GenerateComponentStructure(componentStructures, structureType, [
                new (){ Name = "Title", Value = declaration.Name.AddSpaceBeforeCapital() },
                new (){ Name = "JsonProperty", Value = declaration.Name.ToCamelCase() }
            ]);
        }
        private static Structure GenerateTableStructure(List<ComponentStructure> componentStructures, string name)
        {
            return GenerateComponentStructure(componentStructures, "Table", [
                new (){ Name = "Form", Type = "Component" },
                new (){ Name = "Fields", Type = "Component" },
                new (){ Name = "Title", Value = name.ToTitleCase() },
                new (){ Name = "ServerSidePagination", Type = nameof(Boolean), Value = "false" },
                new (){ Name = "EditAction", Type = nameof(Boolean), Value = "true" },
                new (){ Name = "DeleteAction", Type = nameof(Boolean), Value = "true" }
            ]);
        }
        #endregion

        #region [Form]
        private static Structure GenerateFormFieldStructure(List<ComponentStructure> componentStructures, PropertyDeclaration declaration, string structureType)
        {
            return GenerateComponentStructure(componentStructures, structureType, [
                new (){ Name = "Title", Value = declaration.Name.AddSpaceBeforeCapital() },
                new (){ Name = "JsonProperty", Value = declaration.Name.ToCamelCase() },
                new (){ Name = "Checked", Type = "Boolean", Value = declaration.Value }
            ]);
        }
        private static Structure GenerateFormStructure(List<ComponentStructure> componentStructures, string name)
        {
            return GenerateComponentStructure(componentStructures, "Form", [
               new (){ Name = "Fields", Type = "Component" },
               new (){ Name = "Title", Value = name.ToTitleCase() }
            ]);
        }
        #endregion
        private static Structure FillProperties(List<ComponentStructure> componentStructures, Structure structure)
        {
            ComponentStructure componentStructure = componentStructures.FirstOrDefault(d => d.Id == structure.ComponentStructureId);
            structure.Properties.ForEach(d => d.ComponentStructureProperty = componentStructure.Properties.FirstOrDefault(p => p.Id == d.ComponentStructurePropertyId));
            return structure;
        }
        private static int StructurePropertyId(List<ComponentStructure> componentStructures, Structure structure, string name)
        {
            return structure.Properties.FirstOrDefault(s => s.ComponentStructurePropertyId == componentStructures.FirstOrDefault(d => d.Id == structure.ComponentStructureId).Properties.FirstOrDefault(d => d.ComponentProperty.Name == name).Id).Id;
        }
        private static Structure GenerateComponentStructure(List<ComponentStructure> componentStructures, string name, List<PropertyDeclaration> propertyDeclarations)
        {
            ComponentStructure componentStructure = componentStructures.FirstOrDefault(d => d.Name == name);
            return new Structure()
            {
                ComponentStructureId = componentStructure.Id,
                Identifier = $"{componentStructure.Name}-{RandomProvider.RandomString(5)}",
                Properties = componentStructure.Properties.Select(d => new StructureProperty
                {
                    ComponentStructurePropertyId = d.Id,
                    Value = propertyDeclarations.FirstOrDefault(p => p.Name == d.ComponentProperty.Name).Value ?? "",
                    Type = propertyDeclarations.FirstOrDefault(p => p.Name == d.ComponentProperty.Name).Type ?? nameof(String)
                }).ToList()
            };
        }
    }
}