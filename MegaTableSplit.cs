using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace oaiharvester
{
    public class MegaTableSplit
    {
        public MegaTableSplit() { }

        public void splitToAuthorSmall()
        {
            using (var dbArtVeryFull = new ArticleVeryFullMetadataContext())
            {
                using (var tblAuthSmall = new AuthorSmallMetadataContext())
                {
                    char[] delimeters = { ',' };
                    var query = from vFullArticle in dbArtVeryFull.T_ArticleVeryFullMeDa
                                select vFullArticle;
                    foreach (var names in query)
                    {
                        foreach (var name in names.Creator.Split(delimeters))
                        {
                            var authTupl = new T_AuthorSmallMeDa();
                            authTupl.Title = names.Title;
                            authTupl.PubDate = names.PubDate;
                            authTupl.Author = name;
                            authTupl.Link = names.Relation;
                            tblAuthSmall.T_AuthorSmallMeDa.Add(authTupl);
                            try
                            {
                                tblAuthSmall.SaveChanges();
                            }
                            catch (DbUpdateException dupe)
                            {
                                tblAuthSmall.T_AuthorSmallMeDa.Remove(authTupl); 
                                Console.WriteLine(dupe.ToString());
                            }
                            catch (DbEntityValidationException e)
                            {
                                tblAuthSmall.T_AuthorSmallMeDa.Remove(authTupl); 
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
                                Console.WriteLine(e.ToString());
                            }
                        }                      
                    }
                }
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }
        public void splitToSubjectSmall()
        {
            using (var dbArtVeryFull = new ArticleVeryFullMetadataContext())
            {
                using (var tblSubjSmall = new SubjectSmallMetadataContext())
                {
                    char[] delimeters = { ',' };
                    var query = from vFullArticle in dbArtVeryFull.T_ArticleVeryFullMeDa
                                select vFullArticle;
                    foreach (var subjs in query)
                    {
                        foreach (var sub in subjs.Subject.Split(delimeters))
                        {
                            var subjTupl = new T_SubjectSmallMeDa();
                            subjTupl.Title = subjs.Title;
                            subjTupl.PubDate = subjs.PubDate;                            
                            subjTupl.Subject = sub;
                            subjTupl.Link = subjs.Relation;
                            tblSubjSmall.T_SubjectSmallMeDa.Add(subjTupl);
                            try
                            {
                                tblSubjSmall.SaveChanges();
                            }
                            catch (DbUpdateException dupe)
                            {
                                tblSubjSmall.T_SubjectSmallMeDa.Remove(subjTupl);
                                continue;
                            }
                            catch (DbEntityValidationException e)
                            {
                                tblSubjSmall.T_SubjectSmallMeDa.Remove(subjTupl);
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
                                continue;
                            }
                        }
                        
                    }


                }


                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }
    }
}
