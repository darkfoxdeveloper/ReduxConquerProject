using Redux.Database.Readers;
using Redux.Game_Server;
using Redux.Login_Server;

namespace Redux
{
    public static class Program
    {
        public static LoginServer Login;
        public static GameServer Game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!File.Exists(Environment.MachineName + ".ini"))
            {
                Console.WriteLine("Please type a valid IPv4 Address");
                string IPV4Address = Console.ReadLine();
                string MachineName = Environment.MachineName;
                Console.WriteLine("Please type a server name");
                string ServerName = Console.ReadLine();
                Console.WriteLine("Please type a valid database name");
                string DatabaseName = Console.ReadLine();
                Console.WriteLine("Please type a valid database username");
                string DatabaseUsername = Console.ReadLine();
                Console.WriteLine("Please type a valid database password");
                string DatabasePwd = Console.ReadLine();
                var writer = new StreamWriter(File.Create(MachineName + ".ini"));
                writer.WriteLine("[GENERAL]");
                writer.WriteLine("SERVER_NAME=" + ServerName);
                writer.WriteLine("GAME_IP=" + IPV4Address);
                writer.WriteLine("LOGIN_PORT=9958");
                writer.WriteLine("GAME_PORT=5816");
                writer.Close();

                writer = new StreamWriter(File.Create(MachineName + ".cfg.xml"));
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                writer.WriteLine("<hibernate-configuration  xmlns=\"urn:nhibernate-configuration-2.2\" >");
                writer.WriteLine("  <session-factory name=\"NHibernateProject\">");
                writer.WriteLine("    <property name=\"connection.driver_class\">NHibernate.Driver.MySqlDataDriver</property>");
                writer.WriteLine("    <property name=\"connection.connection_string\">");
                writer.WriteLine("      Database=" + DatabaseName + ";Data Source=localhost;User Id=" + DatabaseUsername + ";Password=" + DatabasePwd);
                writer.WriteLine("    </property>");
                writer.WriteLine("    <property name=\"dialect\">NHibernate.Dialect.MySQLDialect</property>");
               // writer.WriteLine("    <property name=\"proxyfactory.factory_class\">NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu</property>");
                writer.WriteLine("  </session-factory>");
                writer.WriteLine("</hibernate-configuration>");
                writer.Close();
                Console.WriteLine("Configuration complete! Server will now start");
            }

            Database.ServerDatabase.InitializeSql();

            foreach (var sob in Database.ServerDatabase.Context.SOB.GetSOBByMap(1039))
            {
                if (sob.Mesh / 10 % 3 == 0)
                {
                    sob.Level = (byte)(20 + (sob.Mesh - 427) / 30 * 5);
                    Database.ServerDatabase.Context.SOB.AddOrUpdate(sob);
                }

            }

            SettingsReader.Read();

            //Begin login server
            Login = new LoginServer("AuthServer", Constants.LOGIN_PORT);

            //Begin game server
            Game = new GameServer("GameServer", Constants.GAME_PORT);

            Console.WriteLine(Constants.SERVER_NAME + " Ready to log in");
        }
    }
}
