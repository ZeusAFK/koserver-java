namespace KnightOnline.Domain.Enums
{
    public enum LoginResult : byte // Or int if values might exceed 255
    {
        AuthSuccess = 0x01,
        AuthNotFound = 0x02,
        AuthInvalid = 0x03,
        AuthBanned = 0x04,
        AuthInGame = 0x05,
        AuthError = 0x06,
        AuthAgreement = 0x0F,
        AuthFailed = 0x0F // Same value as AuthAgreement
    }
}
