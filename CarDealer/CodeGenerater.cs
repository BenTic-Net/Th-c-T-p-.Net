using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarDealer.Models;
namespace CarDealer
{
    public class CodeGenerater
    {
        ApplicationDbContext context = new ApplicationDbContext();

        
        
       
        public void GenerateToDb()
        {
            List<char> Resource = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'X', 'Y', 'Z', 'T', 'U', 'V' };
            Random rand = new Random();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [VerifyCodes]");context.SaveChanges();
            for (int j = 0; j < 100; j++)
            {
               
                VerifyCode Randomstring = new VerifyCode();
                Randomstring.Code="";
                for (int i = 0; i < 6; i++)
                {
                    int randnumber = rand.Next(0, Resource.Count() - 1);
                    Randomstring.Code = Resource[randnumber] + Randomstring.Code;
                }
                
                if (context.VerifyCodes.Find(Randomstring.Code)==null)
                {
                    context.VerifyCodes.Add(Randomstring); context.SaveChanges();
                }
                else j--;
            }

        }
        public string GetCode()
        {
            Random rand = new Random();
            return context.VerifyCodes.ToList()[rand.Next(0, 100)].Code;
        }





    }
}