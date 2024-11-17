using Godot;
using OllamaSharp;
using System;
using System.IO;
using System.Threading;

public partial class MainUi : Control
{
	OllamaApiClient OllamaClient;
	bool Generating = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OllamaClient = new OllamaApiClient(new Uri(GetNode<TextEdit>("ApiUri").Text));
		OllamaClient.SelectedModel = GetNode<TextEdit>("SelectedModel").Text;
	}
	public async void StreamGenerate()
	{
		await foreach (var Stream in OllamaClient.GenerateAsync(GetNode<TextEdit>("History").Text))
			GetNode<TextEdit>("History").Text += Stream.Response;
	}

	public void _on_button_apply_api_uri_pressed()
	{
		OllamaClient = new OllamaApiClient(new Uri(GetNode<TextEdit>("ApiUri").Text));
	}

	public void _on_button_generate_pressed()
	{
		GetNode<TextEdit>("History").Text += "\n" + GetNode<TextEdit>("InjectResponse").Text;
		StreamGenerate();
	}

	public void _on_button_send_pressed()
	{
		GetNode<TextEdit>("History").Text += "\n" + GetNode<TextEdit>("InjectInput").Text + GetNode<TextEdit>("Input").Text;
		GetNode<TextEdit>("Input").Text = "";
		GetNode<TextEdit>("History").Text += "\n" + GetNode<TextEdit>("InjectResponse").Text;
		StreamGenerate();
	}

	public void _on_selected_model_text_changed()
	{
		OllamaClient.SelectedModel = GetNode<TextEdit>("SelectedModel").Text;
	}

	public void _on_button_quit_pressed()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
