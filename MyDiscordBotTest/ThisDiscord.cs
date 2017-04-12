using Discord;
using Discord.Commands;
using Discord.Net;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*  Discord Bot Command List:
 *  !Help -> Shows all the commands of the bot
 *  !Purge -> Cleans the chat for ... items
 *  !Image -> shows image (not random yet)
 *  !Bye user.Name -> Says Bye to user
 *  !Hello user.Name -> Says Hello to user
 *  
 *  Other Things:
 *  The Bot shows when someone is Banned/UnBanned or Joined/Left The Channel
 */


namespace MyDiscordBotTest
{
    public class ThisDiscord
    {
        DiscordClient discord;
        CommandService commands;
        const string discordToken = "MzAwOTE0MjY0MzI2Mjc1MDgx.C8z43A.2QORFn54AgYB957nlZJ8aesz85Y";
        public ThisDiscord()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();

            CommandLine();
            PlayerJoined();
            PictureLine();
            TimeToPurge();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(discordToken, TokenType.Bot);
            });

        }

        private void CommandLine()
        {
            commands.CreateCommand("hello")
                .Description("The Hello Command")
                .Parameter("GreetsUser", ParameterType.Required)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(e.User + " Says Hello to " + e.GetArg("GreetsUser"));
                });

            commands.CreateCommand("bye")
                .Description("GreetsUser")
                .Parameter("ByeUser", ParameterType.Required)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(e.User + " Says Goodbye to " + e.GetArg("ByeUser")); //Command !Bye 'Name'
                });

            commands.CreateCommand("help")
                .Do(async (e) =>
            {
                //await e.Command.Description
                await e.Channel.SendMessage("We as a community will help you!! ... and the full command list will come shortly...");
            });

        }

        private void PlayerJoined()
        {
            //User Joined
            discord.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendMessage(string.Format("{0} has joined the channel", user.Name));
            };
            //User Left
            discord.UserLeft += async (s, e) =>
            {

                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendMessage(string.Format("{0} has left the channel", user.Name));
            };
            //User Banned
            discord.UserBanned += async (s, e) =>
            {

                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendMessage(string.Format("{0} has been given the Ban-Hammer", user.Name));
            };
            //User UnBanned
            discord.UserUnbanned += async (s, e) =>
            {

                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendMessage(string.Format("{0} has been given unbanned", user.Name));
            };
        }

        private void PictureLine()
        {
            commands.CreateCommand("image").Do(async (e) =>
            {
                await e.Channel.SendFile("Pictures/Zelda.jpg"); //Properties of the image must be: Content, Copy to newer
            });
        }

        private void TimeToPurge()
        {
            commands.CreateCommand("purge")
                .Description("The Purge Command")
                .Do(async (e) =>
                {
                    Message[] messagesToDelete;
                    messagesToDelete = await e.Channel.DownloadMessages(100); //Delete Count recommendation is 100 above can screw things for your bot...

                    await e.Channel.DeleteMessages(messagesToDelete);
                    await e.Channel.SendMessage("We Have Been Purged!!");
                });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
