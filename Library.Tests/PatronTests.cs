using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using Library;

namespace Library.Tests
{
    [TestClass]
    public class PatronTest : IDisposable
    {
        public PatronTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }
        public void Dispose()
        {
            Patron.DeleteAll();
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Int()
        {
            Patron testPatron = new Patron("Test Name");
            testPatron.Save();
            int expected = 1;
            Patron foundPatron = Patron.FindPatron(testPatron.Id);
            int actual = foundPatron.Id;
            Assert.AreEqual(expected, actual);
        }
    }
}
