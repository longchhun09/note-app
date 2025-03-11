using Microsoft.EntityFrameworkCore;
using NoteApi.Data;
using NoteApi.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace NoteApi.Data
{
    public static class DbSeeder
    {
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static void SeedData(NoteDbContext context)
        {
            // Only seed if both users and notes are empty
            if (context.Users.Any() && context.Notes.Any())
            {
                return; 
            }
            // Seed a default admin user if no users exist
            User adminUser = null;
            if (!context.Users.Any())
            {
                CreatePasswordHash("Admin123!", out byte[] passwordHash, out byte[] passwordSalt);
                
                adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.UtcNow,
                    RefreshToken = string.Empty,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7),
                    IsActive = true
                };
                
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
            else
            {
                // If users exist but notes don't, get the first user to associate notes with
                adminUser = context.Users.FirstOrDefault();
            }

            // Only seed notes if none exist
            if (!context.Notes.Any())
            {
                var notes = new Note[]
            {
                new Note
                {
                    Title = "Welcome to Notes App",
                    Content = "This is a sample note to get you started. You can create, edit, and delete notes as needed.",
                    UserId = adminUser?.Id
                },
                new Note
                {
                    Title = "Getting Started",
                    Content = "To create a new note, click on the 'New Note' button and fill in the title and content fields.",
                    UserId = adminUser?.Id
                },
                new Note
                {
                    Title = "Features",
                    Content = "This app allows you to create, read, update, and delete notes. All your notes are stored securely in a database.",
                    UserId = adminUser?.Id
                },
                new Note
                {
                    Title = "Todo List",
                    Content = "1. Buy groceries\n2. Call mom\n3. Finish project\n4. Exercise",
                    UserId = adminUser?.Id
                },
                new Note
                {
                    Title = "Meeting Notes",
                    Content = "Discussed project timeline and deliverables. Next meeting scheduled for Friday.",
                    UserId = adminUser?.Id
                }
            };

                context.Notes.AddRange(notes);
                context.SaveChanges();
            }
        }
    }
}

