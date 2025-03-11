using Microsoft.EntityFrameworkCore;
using NoteApi.Data;
using NoteApi.Models;
using System;
using System.Linq;

namespace NoteApi.Data
{
    public static class DbSeeder
    {
        public static void SeedData(NoteDbContext context)
        {
            if (context.Notes.Any())
            {
                return; 
            }

            var notes = new Note[]
            {
                new Note
                {
                    Title = "Welcome to Notes App",
                    Content = "This is a sample note to get you started. You can create, edit, and delete notes as needed."
                },
                new Note
                {
                    Title = "Getting Started",
                    Content = "To create a new note, click on the 'New Note' button and fill in the title and content fields."
                },
                new Note
                {
                    Title = "Features",
                    Content = "This app allows you to create, read, update, and delete notes. All your notes are stored securely in a database."
                },
                new Note
                {
                    Title = "Todo List",
                    Content = "1. Buy groceries\n2. Call mom\n3. Finish project\n4. Exercise"
                },
                new Note
                {
                    Title = "Meeting Notes",
                    Content = "Discussed project timeline and deliverables. Next meeting scheduled for Friday."
                }
            };

            context.Notes.AddRange(notes);
            context.SaveChanges();
        }
    }
}

