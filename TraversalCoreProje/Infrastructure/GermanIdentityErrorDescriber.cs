using Microsoft.AspNetCore.Identity;

namespace TraversalCoreProje.Infrastructure
{
    /// <summary>Deutsche Fehlermeldungen für ASP.NET Core Identity.</summary>
    public class GermanIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email) => new() { Code = nameof(DuplicateEmail), Description = $"Die E-Mail-Adresse '{email}' wird bereits verwendet." };
        public override IdentityError DuplicateUserName(string userName) => new() { Code = nameof(DuplicateUserName), Description = $"Der Benutzername '{userName}' ist bereits vergeben." };
        public override IdentityError InvalidEmail(string email) => new() { Code = nameof(InvalidEmail), Description = $"Die E-Mail-Adresse '{email}' ist ungültig." };
        public override IdentityError InvalidUserName(string userName) => new() { Code = nameof(InvalidUserName), Description = $"Der Benutzername '{userName}' ist ungültig." };
        public override IdentityError PasswordMismatch() => new() { Code = nameof(PasswordMismatch), Description = "Das aktuelle Passwort ist falsch." };
        public override IdentityError PasswordTooShort(int length) => new() { Code = nameof(PasswordTooShort), Description = $"Das Passwort muss mindestens {length} Zeichen lang sein." };
        public override IdentityError DuplicateRoleName(string role) => new() { Code = nameof(DuplicateRoleName), Description = $"Die Rolle '{role}' existiert bereits." };
        public override IdentityError InvalidRoleName(string role) => new() { Code = nameof(InvalidRoleName), Description = $"Der Rollenname '{role}' ist ungültig." };
        public override IdentityError UserAlreadyInRole(string role) => new() { Code = nameof(UserAlreadyInRole), Description = $"Der Benutzer hat die Rolle '{role}' bereits." };
        public override IdentityError UserNotInRole(string role) => new() { Code = nameof(UserNotInRole), Description = $"Der Benutzer hat die Rolle '{role}' nicht." };
        public override IdentityError DefaultError() => new() { Code = nameof(DefaultError), Description = "Ein unbekannter Fehler ist aufgetreten." };
        public override IdentityError ConcurrencyFailure() => new() { Code = nameof(ConcurrencyFailure), Description = "Der Datensatz wurde inzwischen geändert. Bitte erneut versuchen." };
    }
}
