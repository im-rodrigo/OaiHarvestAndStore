using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using oaiharvester;
using oai;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

//http://doaj.org/oai.article?verb=ListRecords&from=2004-07-16&metadataPrefix=oai_dc
namespace oaiharvester
{
    class Program
    {
        static void Main(string[] args)
        {
            oai.OAI objOAI = new oai.OAI("http://doaj.org/oai.article");

            ListRecord objRecord = new ListRecord();
            ListIdentifier objIdentifier = new ListIdentifier();

            DateTime fromdate = new DateTime(2004, 1, 1);
            for (int day = 9; day < 26; ++day)
            {
                DateTime thisdate = new DateTime(2014, 3, day);

                objRecord = objOAI.ListRecords("", fromdate.ToString("yyyy-MM-dd"), thisdate.ToString("yyyy-MM-dd"));
                objIdentifier = objOAI.ListIdentifiers("");
                ////Console.WriteLine("Record:" + objRecord.record);

                //To hold duplicate sql tuples not inserted
                System.Collections.ArrayList duprecords = new System.Collections.ArrayList();

                //process through all records (100 per unique search
                foreach (var item in objRecord.record)
                {
                    //per record, make a db context object to do an insert. 
                    using (var db = new ArticleVeryFullMetadataContext())
                    {
                        var articlemeda = new T_ArticleVeryFullMeDa();

                        Record rec = (Record)item;
                        OAI_DC metadatalist = (OAI_DC)rec.metadata;
                        StringBuilder tmpbuilder = new StringBuilder();
                        foreach (var ttl in metadatalist.title)
                        {
                            tmpbuilder.Append(ttl.ToString());
                            tmpbuilder.Append(", ");

                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Title = tmpbuilder.ToString();
                        Console.WriteLine("Title: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();
                        //Console.WriteLine("Creator (Authors): ");
                        foreach (var creator in metadatalist.creator)
                        {
                            if (creator.ToString().Length < 40)
                            {
                                tmpbuilder.Append(creator.ToString());
                                tmpbuilder.Append(", ");
                            }
                            else
                            {
                                tmpbuilder.Append(creator.ToString().Substring(0, 40));
                                tmpbuilder.Append(", ");
                            }
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Creator = tmpbuilder.ToString();
                        //Console.WriteLine("Creator: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var desc in metadatalist.description)
                        {
                            tmpbuilder.Append(desc.ToString());
                            tmpbuilder.Append(", ");
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Description = tmpbuilder.ToString();
                        //Console.WriteLine("Description (abstract): " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var pub in metadatalist.publisher)
                        {
                            tmpbuilder.Append(pub.ToString());
                            tmpbuilder.Append(", ");
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        articlemeda.Publisher = tmpbuilder.ToString();
                        //Console.WriteLine("Publisher: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        //Console.WriteLine("Identifiers (Id numbers):");
                        //If inserting to T_ArticleVeryFullMeDa
                        int count = 0;
                        foreach (var ids in metadatalist.identifier)
                        {
                            tmpbuilder.Append(ids.ToString());
                            if (count == 0)
                                articlemeda.aSSN = tmpbuilder.ToString();
                            else if (count == 1)
                                articlemeda.bSSN = tmpbuilder.ToString();
                            else if (count == 2)
                                articlemeda.ExtraId1 = tmpbuilder.ToString();
                            else if (count == 3)
                                articlemeda.ExtraId2 = tmpbuilder.ToString();
                            //Console.WriteLine("Identifiers: " + tmpbuilder.ToString());
                            tmpbuilder.Clear();
                            ++count;
                        }
                        ////Console.WriteLine("Identifiers: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();
                        //If inserting to T_ArticleFullMeDa
                        /*
                        foreach (var ids in metadatalist.identifier)
                        {
                            tmpbuilder.Append(ids.ToString());
                            tmpbuilder.Append(", ");
                        }
                        tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Identifier = tmpbuilder.ToString();
                        //Console.WriteLine("Identifiers: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();
                        */
                        foreach (var lang in metadatalist.language)
                        {
                            tmpbuilder.Append(lang.ToString());
                            tmpbuilder.Append(", ");
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.PubLanguage = tmpbuilder.ToString();
                        //Console.WriteLine("Language: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var rel in metadatalist.relation)
                        {
                            tmpbuilder.Append(rel.ToString());
                            tmpbuilder.Append(", ");
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Relation = tmpbuilder.ToString();
                        //Console.WriteLine("Relation (Source): " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var sub in metadatalist.subject)
                        {
                            if (sub.ToString().Length < 40)
                            {
                                tmpbuilder.Append(sub.ToString());
                                tmpbuilder.Append(", ");
                            }
                            else
                            {
                                tmpbuilder.Append(sub.ToString().Substring(0, 40));
                                tmpbuilder.Append(", ");
                            }
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Subject = tmpbuilder.ToString();
                        //Console.WriteLine("Subjects:" + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var typ in metadatalist.type)
                        {
                            tmpbuilder.Append(typ.ToString());
                            tmpbuilder.Append(", ");
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        articlemeda.Type = tmpbuilder.ToString();
                        //Console.WriteLine("Type: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();

                        foreach (var dt in metadatalist.date)
                        {
                            tmpbuilder.Append(dt.ToString());
                            tmpbuilder.Append(", ");
                            //Console.WriteLine("PubDate: " + dt.ToString());
                        }
                        try
                        {
                            tmpbuilder.Remove(tmpbuilder.Length - 2, 2);
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            tmpbuilder.Append("N/A");
                        }
                        //tmpbuilder.Remove(tmpbuilder.Length - 2, 2);'
                        System.DateTime tmpdate = new System.DateTime();

                        try
                        {

                            tmpdate = System.DateTime.ParseExact(tmpbuilder.ToString(), "yyyy-MM-ddTHH:mm:ssZ", null);
                            articlemeda.PubDate = tmpdate;
                        }
                        catch (System.FormatException e)
                        {
                            tmpbuilder.Append("N/A");
                        }

                        //Console.WriteLine("PubDate: " + tmpbuilder.ToString());
                        tmpbuilder.Clear();
                        //2013-12-01T00:00:00Z
                        db.T_ArticleVeryFullMeDa.Add(articlemeda);

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);


                                }
                            }
                            throw;
                        }
                        catch (DbUpdateException dbupe)
                        {
                            //UpdateException upe = new UpdateException();
                            var innexc = dbupe.InnerException;
                            if ((innexc.GetType()) == typeof(UpdateException))
                            {
                                var sqlinnexc = innexc.InnerException;
                                if ((sqlinnexc.GetType()) == typeof(SqlException))
                                {
                                    SqlException sex = (SqlException)sqlinnexc;

                                    switch (sex.Number)
                                    {
                                        case 242:
                                            duprecords.Add(articlemeda);
                                            break;
                                        case 2601:
                                            duprecords.Add(articlemeda);
                                            break;
                                        case 2627:
                                            duprecords.Add(articlemeda);
                                            break;
                                        default:
                                            throw;

                                    }
                                }

                            }
                        }
                        //db.SaveChanges();
                    }

                }

            }
            Console.ReadKey();
        }

    }
}
