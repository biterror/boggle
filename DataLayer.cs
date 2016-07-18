using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Boggle
{
    class DataLayer
    {
        private SqlConnection Connection;


        public SqlConnection Connect()
        {
            Connection = new SqlConnection(@"Server=tcp:p6rbnv719l.database.windows.net,1433;Database=boggle;User ID = ipd7abbott@p6rbnv719l;Password=JohnAbbott2000; Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;");
            Connection.Open();
            return Connection;
        }

        public void AddBoard(String letters) {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "INSERT INTO Boards (Letters) VALUES (@Letters)";
                cmd.Parameters.AddWithValue("@Letters", letters);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = Connection;
                cmd.ExecuteNonQuery();
            }

        }
        public void AddScore(String name, int score, int boardid)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "INSERT INTO Score (Name, Score, BoardID) VALUES (@Name, @Score, @BoardID)";
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.Parameters.AddWithValue("@BoardID", boardid);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = Connection;
                cmd.ExecuteNonQuery();
            }

        }

        public List<Board> GetAllBoards()
        {
            List<Board> result = new List<Board>();
            using (SqlCommand cmd = new SqlCommand("SELECT Boards.ID, Boards.Letters, Score.Name, Score.Score FROM Boards INNER JOIN Score ON Boards.ID=Score.BoardID ORDER BY Score.Score DESC", Connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string letters = reader.GetString(1);
                        string name = reader.GetString(2);
                        int hscore = reader.GetInt32(3);
                        Board b = new Board(ID, letters, hscore, name);
                        result.Add(b);
                    }
                }
            }
            return result;
        }

  
    }




    public class Board
    {
        public int BoardId { get; set; }
        public string Letters { get; set; }
        public int HScore { get; set; }
        public string HScoreName { get; set; }
        public Board(int ID, String letter, int hscore, string hscorename)
        {
            BoardId = ID;
            Letters = letter;
            HScore = hscore;
            HScoreName = hscorename;

        }

    }
}
