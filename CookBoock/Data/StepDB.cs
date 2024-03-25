using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBoock.Helpers;
using CookBoock.Models;
using LiteDB;

namespace CookBoock.Data
{
    internal class StepDB : DB<Step>
    {
        private ILiteStorage<string> Fs;
        private ILiteCollection<Step> Steps;
        private LiteFileStream<string> ImageStream;

        public int StepsCount
        {
            get
            {
                return Steps.Count();
            }
        }

        public StepDB(string dbPath) : base(dbPath)
        {
            Fs = Db.FileStorage;
            Steps = Db.GetCollection<Step>();
        }

        public void Update(Step step, Stream stream)
        {
            if (stream != null)
            {
                Fs.Upload(step.FileId, Guid.NewGuid().ToString(), stream);
            }
            Steps.Update(step);
        }

        public override void Add(Step step, Stream stream = null)
        {
            step.Image = null;
            if (stream != null)
            {
                step.SetFileId();
                Fs.Upload(step.FileId, Guid.NewGuid().ToString(), stream);
            }
            Steps.Insert(step);
        }

        public override void ClearAll()
        {
            Steps.DeleteAll();
        }

        public override List<Step> GetAll()
        {
            List<Step> res = Steps.FindAll().ToList();

            Parallel.For(0, res.Count(), (int i) =>
            {
                //res[i].Image = GetImage(res[i].FileId);
            });
            ImageGeter.ClearImageCashe();
            return res;
        }

        public override Step GetById(int id)
        {
            return null;
        }

        public ObservableCollection<Step> GetByListOfId(List<int> ids)
        {
            ObservableCollection<Step> res = new ObservableCollection<Step>();
            Step buf = null;
            foreach (int id in ids)
            {
                //var a = Steps.Exists(x => x.Id == id);
                //buf = Steps.FindOne(x => x.Id == id);
                //buf.Image = GetImage(buf.FileId);
                res.Add(buf);
            }
            return res;
        }

        public LiteFileStream<string> GetStream(string fileId)
        {
            return FindImageById(fileId);
            //return null;  
        }

        public Microsoft.Maui.Graphics.IImage GetImage(string uuid)
        {
            try
            {
                var stream = FindImageById(uuid);
                Microsoft.Maui.Graphics.IImage image = null;
                image = ImageGeter.GetImage(stream);
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void DeleteById(int id)
        {
            var step = GetById(id);
            Fs.Delete(step.FileId);
            Steps.Delete(id);
        }

        public LiteFileStream<string> FindImageById(string id)
        {
            ImageStream = Fs.OpenRead(id);
            return ImageStream;
        }
    }
}
