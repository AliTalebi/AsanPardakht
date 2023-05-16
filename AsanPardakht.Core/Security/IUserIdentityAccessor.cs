namespace AsanPardakht.Core.Security
{
    public interface IUserIdentityAccessor
    {
        string? GetUserName();
        string? GetNationalCode();
        int GetSubjectId();
    }
}
