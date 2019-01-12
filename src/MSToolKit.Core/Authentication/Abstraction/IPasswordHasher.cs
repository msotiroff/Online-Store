namespace MSToolKit.Core.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for hashing passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns hashed representation of the supplied password.
        /// </summary>
        /// <param name="input">The password in plain text.</param>
        /// <returns>Hashed input.</returns>
        string ComputeHash(string input);
    }
}
