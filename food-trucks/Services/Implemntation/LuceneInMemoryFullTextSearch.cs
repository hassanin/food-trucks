using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

using System.IO;
using System.Text;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Microsoft.Extensions.Logging;

namespace food_trucks.Services.Implemntation
{
    public class LuceneInMemoryFullTextSearch : IFullTextSearchFoodTruck, IDisposable
    {
        private FSDirectory dir;
        private IndexWriter writer;
        private ILogger logger;
        public LuceneInMemoryFullTextSearch(ILogger<LuceneInMemoryFullTextSearch> logger) 
        {
            this.logger = logger;
            // Ensures index backward compatibility
            const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_CURRENT;

            // Construct a machine-independent path for the index
            var basePath = Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData);
            var indexPath = Path.Combine(basePath, "index");

            this.dir = FSDirectory.Open(indexPath);

            // Create an analyzer to process the text
            var analyzer = new StandardAnalyzer(AppLuceneVersion);

            // Create an index writer
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            this.writer = new IndexWriter(dir, indexConfig);
        }
        public void AddFoodTruckDescrption(string foodTruckId, string description)
        {
            var doc = new Document
            {
            // StringField indexes but doesn't tokenize
                new StringField("truckID",
                   foodTruckId,
                    Field.Store.YES),
                new TextField("description", // TextField will toknize here
                   description,
                    Field.Store.YES)
              };
            writer.AddDocument(doc);
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

      
      
        public List<string> GetDocumentIds(List<string> terms, int maxItems=10)
        {
            List<string> result = new List<string>();
            var description = new MultiPhraseQuery();
              
            foreach(var term in terms)
            {
                description.Add(new Term("description", term));
            }

            // Re-use the writer to get real-time updates
            using var reader = writer.GetReader(applyAllDeletes: true);
            var searcher = new IndexSearcher(reader);
            var hits = searcher.Search(description, maxItems).ScoreDocs;
            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                string truckID = foundDoc.Get("truckID");
                string descriptionText = foundDoc.Get("descriptopn");
                result.Add(truckID);
                logger.LogInformation($"{hit.Score:f8}" +
                    $" {foundDoc.Get("truckID"),-15}" +
                    $" {foundDoc.Get("description"),-40}");
            }
            return result;
        }

        public void RemoveFoodTruck(string foodTruck)
        {
            var truckIDQuery = new MultiPhraseQuery() { new Term("truckID", foodTruck) };
            writer.DeleteDocuments(truckIDQuery);
        }

        private bool disposed = false;
        public void Dispose()
        {
            lock (this)
            {
                if (!disposed)
                {
                    writer.Dispose();
                    dir.Dispose();
                }
                disposed = true;
            }
        }
        ~LuceneInMemoryFullTextSearch()
        {
            this.Dispose();
        }

    }
}
