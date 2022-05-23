using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Models
{
    public class NotesModel
    {
        [JsonProperty("id", ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public string NoteId { get; set; } = "";

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("color")]
        public string Color { get; set; } = "";

        [JsonProperty("Image")]
        public string Image { get; set; } = "";

        [JsonProperty("isArchive")]
        public bool IsArchive { get; set; } 

        [JsonProperty("isTrash")]
        public bool IsTrash { get; set; }

        [JsonProperty("isPin")]
        public bool IsPinned { get; set; }
        [JsonProperty("reminder")]
        public DateTime? Reminder { get; set; } 

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }
    }
}
