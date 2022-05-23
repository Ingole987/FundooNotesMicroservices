using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Interface
{
    public interface INoteInterface
    {
        public Task<NotesModel> CreateNotes(NotesModel userNotes, string Id);
        public Task<NotesModel> UpdateNote( NotesUpdate updateNote, string noteId);
        public NotesModel DeleteNote(string noteId);
        public NotesModel GetNoteById(string noteId);
        public Task<List<NotesModel>> GetAllNotes();
        public NotesModel IsPinned( string noteId);
        public NotesModel IsTrash(string noteId);
        public NotesModel IsArchive(string noteId);
        //public NotesModel ColorChange(string noteID, string color);
        //public NotesModel UploadImage(string noteId);
    }
}
