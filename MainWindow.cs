using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gtk;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TLSharp.Core.Network;
using TLSharp.Core.Requests;
using TLSharp.Core.Utils;

public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	System.Random rnd;
	TelegramClient client;
	String hash;
	int timer = 40;

	protected async void OnButtonSendCodeClicked(object sender, EventArgs e)
	{
		var store = new FileSessionStore();
		try
		{
			client = new TelegramClient(129258, "069de88344198a3466583ccfc2749cf1", store, "session");
			await client.ConnectAsync();
		}
		catch (Exception eexc)
		{
			System.Console.Write(eexc.ToString());
		}
		if (!client.IsUserAuthorized())
		{
			client = new TelegramClient(129258, "069de88344198a3466583ccfc2749cf1");
			await client.ConnectAsync();
			try
			{
				rnd = new Random();
				Thread.Sleep(300 + rnd.Next(15));
			}
			catch (Exception eexc)
			{
				System.Console.Write(eexc.ToString());
			}
			try
			{
				hash = await client.SendCodeRequestAsync(entry_num.Text);
			}
			catch (Exception eexc)
			{
				System.Console.Write(eexc.ToString());
			}
		}
		else
			statusview.Buffer.Text = "vhod uzhe byl vypolnen \n aktivirovana turboohota";
	}

	protected async void OnButtonLoginClicked(object sender, EventArgs e)
	{
		var user = await client.MakeAuthAsync(entry_num.Text, hash, entry_code.Text);
		if (client.IsUserAuthorized())
			statusview.Buffer.Text = "vhod vypolnen \n aktivirovana turboohota";
	}

	protected async void OnButtonHuntClicked(object sender, EventArgs e)
	{
		//for (int i = 0; i < hscale_count.Value; i++)
		//{ }
		try
		{
			TeleSharp.TL.Contacts.TLFound found = await this.client.SearchUserAsync("Pokemembro");

			long hasha = ((TeleSharp.TL.TLUser)found.users.lists[0]).access_hash.Value;
			int id = ((TeleSharp.TL.TLUser)found.users.lists[0]).id;
			TeleSharp.TL.TLInputPeerUser peer = new TeleSharp.TL.TLInputPeerUser() { user_id = id, access_hash = hasha };
			for (int i = 0; i < hscale_count.Value; i++)
			{
				System.Random rnd1 = new System.Random();
				TeleSharp.TL.TLAbsUpdates up = await this.client.SendMessageAsync(peer, entry_where.Text);
				Thread.Sleep(timer * 1000 + rnd1.Next(150));
			}
		}
		catch (Exception eexc)
			{
				System.Console.Write(eexc.ToString());
			}
	}

	protected void OnTurboClicked(object sender, EventArgs e)
	{
		timer = 40;
		statusview.Buffer.Text = "turboohota vybrana";
	}

	protected void OnSimpleClicked(object sender, EventArgs e)
	{
		timer = 120;
		statusview.Buffer.Text = "prostaya ohota vybrana";
	}
}
