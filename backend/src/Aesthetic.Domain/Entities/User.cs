using Aesthetic.Domain.Common;
using Aesthetic.Domain.Enums;
using System;

namespace Aesthetic.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }

        // Navigation properties
        public virtual Professional? ProfessionalProfile { get; private set; }

        protected User() 
        {
            FirstName = null!;
            LastName = null!;
            Email = null!;
            PasswordHash = null!;
        }

        public User(string firstName, string lastName, string email, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public void SetProfessionalProfile(Professional professional)
        {
            if (Role != UserRole.Professional)
                throw new InvalidOperationException("Only professionals can have a professional profile.");
            
            ProfessionalProfile = professional;
        }
    }
}
