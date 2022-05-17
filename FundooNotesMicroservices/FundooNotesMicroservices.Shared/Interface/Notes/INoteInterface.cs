using FundooNotesMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Interface
{
    public interface INoteInterface
    {
        public NotesModel CreateNotes(NotesModel userNotes, string Id);
        public Task<NotesModel> UpdateNote(string email, NotesModel updateNote, string noteId);
        public NotesModel DeleteNote(string email, string noteId);
        public NotesModel GetNoteById(string email, string noteId);
        public Task<List<NotesModel>> GetAllNotes(string email);
        public NotesModel IsPinned(string email, string noteId);
        public NotesModel IsTrash(string email, string noteId);
        public NotesModel IsArchive(string email, string noteId);
        //public NotesModel ColorChange(string noteID, string color);
        //public NotesModel UploadImage(string noteId);
    }
}
