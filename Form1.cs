using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private class ListPlaces
    {
      public string StringCodePlace { get; set; }
      public int IntCodePlace { get; set; }
      public int Difference { get; set; }
    }

    private static List<ListPlaces> Nearest(string codeAtualPlaceString, List<ListPlaces> listPlaces)
    {
      int codeAtualPlaceNumber = ParseToNumber(codeAtualPlaceString);

      List<ListPlaces> listPlacesOrdered = new List<ListPlaces>();

      //find the IntCodice and the diference between then and the atual code
      foreach (var item in listPlaces)
      {
        item.IntCodePlace = ParseToNumber(item.StringCodePlace);
        item.Difference = codeAtualPlaceNumber - item.IntCodePlace;
      }

      // first get the place with the same code of the place atual
      // after get the next one (the one in front)
      // example
      // A B C D E F ... AA AB ... AAA AAB ... 1 2
      // if the atual is F, the first one is E (front), after H and etc, etc
      // if the atual is E, the first is F, after D and etc, etc

      //get the place with the same code
      if (listPlaces.Where(x => x.IntCodePlace == codeAtualPlaceNumber).FirstOrDefault() != null)
      {
        listPlacesOrdered.Add(listPlaces.Where(x => x.IntCodePlace == codeAtualPlaceNumber).FirstOrDefault());
        listPlaces.Remove(listPlaces.Where(x => x.IntCodePlace == codeAtualPlaceNumber).FirstOrDefault()); //remove from the original list after include in the orderedlist
      }

      //get the one on the front based on left or right
      if (listPlaces.Where(x => x.Difference == (codeAtualPlaceNumber % 2 == 0 ? 1 : -1)).FirstOrDefault() != null)
      {
        listPlacesOrdered.Add(listPlaces.Where(x => x.Difference == (codeAtualPlaceNumber % 2 == 0 ? 1 : -1)).FirstOrDefault());
        listPlaces.Remove(listPlaces.Where(x => x.Difference == (codeAtualPlaceNumber % 2 == 0 ? 1 : -1)).FirstOrDefault());
      }
      
      //after get the first one, change the signal of the difference for all be over zero 
      listPlaces.Where(c => c.Difference < 0).ToList().Select(c => { c.Difference = (c.Difference * -1); return c; }).ToList();

      //create the new list based on the difference ordered
      foreach (var item in listPlaces.OrderBy(x => x.Difference).ThenBy(z => z.StringCodePlace))
      {
        listPlacesOrdered.Add(item);
      }

      return listPlacesOrdered;

    }

    //The places can be letters, letters and numbers or only numbers
    // example
    // A B C D E F ... AA AB ... AAA AAB ... 1 2
    //the numbers come after the letters
    private static int ParseToNumber(string codePlace)
    {
      int pos = 1;
      if (!string.IsNullOrEmpty(codePlace))
      {
        //if not, means that has letters
        if (!int.TryParse(codePlace, out pos))
        {
          // create array of char for every letter/number
          char[] arrayChar = codePlace.ToUpper().ToArray();

          //if is more than one letters, means that firs come A...Z after AA ... AAA ... AA1, so the position is multiply
          //26 is the maximum letters on alphabet
          if (arrayChar.Length > 1) pos += (26 * (arrayChar.Length - 1));

          //if is a mix letters and numbers
          //example A12, takes A after 12, not A, after 1 and after 2
          string strNumber = string.Empty;
          foreach (var p in arrayChar)
          {
            int n;
            //if il char is number, check if the next one also is number
            if (int.TryParse(p.ToString(), out n)) strNumber += n.ToString();
            else
            {
              if (!string.IsNullOrEmpty(strNumber))
              {
                pos += Convert.ToInt32(strNumber);
                strNumber = string.Empty;
              }
              pos += (p - 65); 
            }
          }
          //se dopo delle lettere c'e numeri, senza lettera alla fine, faccio il sum anche del numero
          if (!string.IsNullOrEmpty(strNumber))
          {
            pos += Convert.ToInt32(strNumber);
          }
        }
      }
      return pos;
    }

    private int abc(char a, char b)
    {
      return 1;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      listView2.Items.Clear();
      List<ListPlaces> listPlaces = new List<ListPlaces>();

      for (int i = 0; i < listView1.Items.Count; i++)
      {
        listPlaces.Add(new ListPlaces { StringCodePlace = listView1.Items[i].Text });
      }

      List<ListPlaces> listPlacesOrdered = Nearest(textBox2.Text, listPlaces);

      foreach (var item in listPlacesOrdered)
      {
        listView2.Items.Add(item.StringCodePlace.ToString());
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      listView1.Items.Add("A");
      listView1.Items.Add("b");
      listView1.Items.Add("c");
      listView1.Items.Add("d");
      listView1.Items.Add("e");
      listView1.Items.Add("f");
      listView1.Items.Add("g");
      listView1.Items.Add("h");
      listView1.Items.Add("i");
      listView1.Items.Add("j");
      listView1.Items.Add("k");
      listView1.Items.Add("l");
      listView1.Items.Add("m");
      listView1.Items.Add("n");
      listView1.Items.Add("o");
      listView1.Items.Add("p");
      listView1.Items.Add("q");
      listView1.Items.Add("r");
      listView1.Items.Add("s");
      listView1.Items.Add("a1");
      listView1.Items.Add("ab");
      listView1.Items.Add("aa");
      listView1.Items.Add("aa1");
      listView1.Items.Add("ab1");
      listView1.Items.Add("ac");

      textBox2.Text = "A";
    }
  }
}
