Imports System.Globalization
Imports System.IO
Imports System.Speech.Recognition
Imports System.Text
Imports System.Threading

Public Class Form1

    Private ReadOnly Drone As New SpeechRecognitionEngine
    Private ReadOnly Qa As New DictationGrammar

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\WM") Then
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\WM")
        End If
        'My.Computer.FileSystem.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\WM")

        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Drone.LoadGrammarAsync(Qa)
        Drone.RequestRecognizerUpdate()
        Drone.SetInputToDefaultAudioDevice()
        Drone.InitialSilenceTimeout = TimeSpan.FromSeconds(2.5)
        Drone.BabbleTimeout = TimeSpan.FromSeconds(1.5)
        Drone.EndSilenceTimeout = TimeSpan.FromSeconds(1.2)
        Drone.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(1.5)
        AddHandler Drone.SpeechRecognized, AddressOf Recognizer_SpeechRecognized

        Drone.RecognizeAsync(RecognizeMode.Multiple)

        'In the event of duplicated word is in the command list >> (ArchiveData.txt), this removes double words
        'Dim path As String = "C:\Users\justin.ross\source\repos\Search Ultima\Search Ultima\ArchiveData.txt"
        'Dim lines As New HashSet(Of String)()
        'Read to file
        'Using sr As New StreamReader(path)
        'Do While sr.Peek() >= 0
        'lines.Add(sr.ReadLine())
        'Loop
        'End Using

        'Write to file
        'Using sw As New StreamWriter(path)
        'For Each line As String In lines
        'sw.WriteLine(line)
        'Next
        'End Using

    End Sub

    Private Sub Recognizer_SpeechRecognized(sender As Object, e As SpeechRecognizedEventArgs)
        Dim speech As String = e.Result.Text.ToString

        Select Case (speech)
            Case speech

                If speech <> "+" Then

                    'This section will take spoken word, write to file then execute search.
                    Dim sb As New StringBuilder

                    sb.AppendLine(speech)
                    File.WriteAllText("C:\Users\justin.ross\source\repos\Search Ultima\Search Ultima\File.txt", sb.ToString())
                    speech = "https://www.youtube.com/search?q=" & Uri.EscapeUriString(speech)
                    Dim proc As New Process()
                    Dim startInfo As New ProcessStartInfo(speech)
                    proc.StartInfo = startInfo
                    proc.Start()

                    Return
                End If
        End Select
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Create Event
    End Sub

    'Disable Engine
    Private Sub Disable_Click(sender As Object, e As EventArgs) Handles Disable.Click
        Drone.RecognizeAsyncStop()
        Dim frm As New ScarlettUltima
        frm.Show()
        Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Computer.Audio.Play(My.Resources.informative, AudioPlayMode.Background)
    End Sub

End Class