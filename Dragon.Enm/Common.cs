namespace Dragon.Enm
{
    public enum StatusFlags : byte
    {
        Success = 1,
        Failed = 2,
        AlreadyExists = 3,
        DependencyExists = 4,
        NotPermitted = 5
    }
}
