using Dragon.Provider;
using System.ComponentModel.DataAnnotations;

namespace Dragon.Model
{
    public struct ApiResponse
    {
        public byte Status { get; set; }
        public string Message { get; set; }
        public string DetailedError { get; set; }
        public object Data { get; set; }
    }
    public struct PageData
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int RecordsCount { get; set; }
        public bool IsClientSide { get; set; }
        public object Data { get; set; }
        public Dictionary<string, object> Filter { get; set; }
    }

    public struct AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string TenantCode { get; set; }
    }
    public abstract class TransectionKeys
    {
        [Key] public int Id { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }

    public struct ReindexStruct
    {
        public int Id { get; set; }
        public int ShortIndex { get; set; }
    }

    public class OptionsTransfer
    {
        public string Table { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string KeyStore { get; set; }

        public string CascadeBy { get; set; }
        public string CascadeValue { get; set; }
        public bool StoreId { get; set; } = true;

        public List<Options> Options { get; set; } = null;
    }
    public struct OptionGType<T, K>
    {
        public T Key { get; set; }
        public K Value { get; set; }
    }
}