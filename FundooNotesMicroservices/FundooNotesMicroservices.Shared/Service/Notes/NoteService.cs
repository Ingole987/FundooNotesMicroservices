using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Shared.Interface;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Service
{
    public class NoteService : INoteInterface
    {
        private string _uri;
        private string _primaryKey;
        private string _databaseName;
        private string _containerName;

        private static CosmosClient? _cosmosClient;
        private readonly ISettingService _settingService;
        private readonly Container _cosmosContainer;
        private readonly IJWTService _jWTService;

        public NoteService(ISettingService setting, IJWTService jWTService)
        {
            _settingService = setting;

            _uri = _settingService.GetUri();
            _primaryKey = _settingService.GetPrimaryKey();
            _databaseName = _settingService.GetDatabaseName();
            _containerName = _settingService.GetContainerName();

            _jWTService = jWTService;

            _cosmosClient = new CosmosClient(_settingService.GetUri(), _settingService.GetPrimaryKey());
            _cosmosContainer = _cosmosClient.GetContainer(_databaseName, _containerName);
            //_cosmosContainer = new Lazy<Task<UserModel>>(async () =>
            //{
            //    var cosmos = new CosmosClient(setting.GetUri(), setting.GetPrimaryKey());
            //    var db = cosmos.GetDatabase(setting.GetDatabaseName());
            //    return await db.CreateContainerIfNotExistsAsync(setting.GetContainerName(), partitionKeyPath: "/email");

            //});
        }
       

        public NotesModel CreateNotes(NotesModel userNotes, string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(_containerName))
                    throw new Exception("No Digital Main collection defined!");
                userNotes.Id = Guid.NewGuid().ToString();
                using (var result = _cosmosContainer.CreateItemAsync<NotesModel>(userNotes, new PartitionKey(userNotes.NoteId)))
                {
                    return result.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                // Detect a `Resource Not Found` exception...do not treat it as error
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public NotesModel GetNoteById(string noteId)
        {
            try
            {
                if (noteId == null)
                {
                    throw new NullReferenceException();
                }

                var note = _cosmosContainer.ReadItemAsync<NotesModel>(noteId, new PartitionKey(noteId));

                return note.Result.Resource;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            
        }

        public async Task<List<NotesModel>> GetAllNotes()
        {
            try
            {
                QueryDefinition query = new QueryDefinition("select * from NoteContainer");


                List<NotesModel> noteLists = new List<NotesModel>();
                using (FeedIterator<NotesModel> resultSet = _cosmosContainer.GetItemQueryIterator<NotesModel>(query))
                {
                    while (resultSet.HasMoreResults)
                    {
                        FeedResponse<NotesModel> response = await resultSet.ReadNextAsync();

                        noteLists.AddRange(response);

                    }
                    return noteLists;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }



        public async Task<NotesModel> UpdateNote(NotesModel updateNote, string noteId)
        {
            if (noteId == null)
            {
                throw new NullReferenceException();
            }

            var result = _cosmosContainer.GetItemLinqQueryable<NotesModel>(true).Where(b => b.NoteId == noteId)
                            .AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                result.Title = updateNote.Title;
                result.Description = updateNote.Description;
                result.Color = updateNote.Color;
                result.Image = updateNote.Image;
                result.ModifiedAt = DateTime.Now;
                var response = await _cosmosContainer.ReplaceItemAsync<NotesModel>(result, result.NoteId, new PartitionKey(result.NoteId));
                return response.Resource;
            }
            return null;
        }

        public NotesModel IsPinned(string noteId)
        {
            if (noteId == null)
            {
                throw new NullReferenceException();
            }

            var result = _cosmosContainer.GetItemLinqQueryable<NotesModel>(true).Where(b => b.NoteId == noteId)
                            .AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                bool checkPinned = result.IsArchive;
                if (checkPinned == true)
                {
                    result.IsPinned = false;
                }
                else
                {
                    result.IsPinned = true;
                }
                _cosmosContainer.ReplaceItemAsync<NotesModel>(result, result.NoteId, new PartitionKey(result.NoteId));
                return result;
            }
            return null;
        }

        public NotesModel IsArchive(string noteId)
        {
            if (noteId == null)
            {
                throw new NullReferenceException();
            }

            var result = _cosmosContainer.GetItemLinqQueryable<NotesModel>(true).Where(b => b.NoteId == noteId)
                            .AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                bool checkIsArchive = result.IsArchive;

                if (checkIsArchive == true)
                {
                    result.IsArchive = false;
                }
                else
                {
                    result.IsArchive = true;
                }
                _cosmosContainer.ReplaceItemAsync<NotesModel>(result, result.NoteId, new PartitionKey(result.NoteId));
                return result;
            }
            return null;
        }

        public NotesModel IsTrash(string noteId)
        {
            if (noteId == null)
            {
                throw new NullReferenceException();
            }

            var result = _cosmosContainer.GetItemLinqQueryable<NotesModel>(true).Where(b => b.NoteId == noteId)
                            .AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                bool checkIsTrash = result.IsArchive;
                if (checkIsTrash == true)
                {
                    result.IsTrash = false;
                }
                else
                {
                    result.IsTrash = true;
                }
                _cosmosContainer.ReplaceItemAsync<NotesModel>(result, result.NoteId, new PartitionKey(result.NoteId));
                return result;
            }
            return null;
        }

        

    }
}
