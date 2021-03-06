'EdSharp 3.1
' April 1, 2009
'Copyright 2006 - 2009 by Jamal Mazrui
'Modified GPL License

imports Microsoft.VisualBasic
imports Microsoft.VisualBasic.MyServices
Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Media
Imports System.Security.Cryptography
Imports System.IO
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Module VB
Public Function ExcelOpen(oXlss As Object, sFile As String) As Object
Dim bUpdateLinks As Boolean = False
Dim bReadOnly As Boolean = True

Dim oXls As Object = Nothing
Try
oXls = oXlss.Open(sFile, bUpdateLinks, bReadOnly)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
Return oXls
End Function 'OpenExcelXlsument method

Public Sub ExcelSaveAs(oXls As Object, sFile As String, iFileFormat As Integer) 
Try 
oXls.SaveAs(sFile, iFileFormat)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' ExcelSaveAs method

Public Sub ExcelClose(oXls As Object) 
Dim oApp As Object = oXls.Application

Dim iSaveChanges As Integer = 0
Try 
oXls.Close(iSaveChanges)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' ExcelClose method

Public Sub ExcelQuit(oApp As Object) 

Dim iSaveChanges As Integer = 0
Try 
oApp.Quit()
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' ExcelQuit method

Public Function PowerPointOpen(oPpts As Object, sFile As String) As Object
Dim bReadOnly As Boolean = True

Dim oPpt As Object = Nothing
Try
oPpt = oPpts.Open(sFile, bReadOnly)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
Return oPpt
End Function 'OpenPowerPointPptument method

Public Sub PowerPointSaveAs(oPpt As Object, sFile As String, iFileFormat As Integer) 
Try 
oPpt.SaveAs(sFile)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' PowerPointSaveAs method

Public Sub PowerPointClose(oPpt As Object) 
Dim oApp As Object = oPpt.Application

Try 
oPpt.Close()
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' PowerPointClose method

Public Sub PowerPointQuit(oApp As Object) 

Try 
oApp.Quit()
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' PowerPointQuit method

Public Function WordOpen(oDocs As Object, sFile As String, bAppVisible As Boolean) As Object
Dim bConfirmConversions As Boolean = false
Dim bReadOnly As Boolean = false
Dim bAddToRecentFiles As Boolean = false

Dim oDoc As Object = Nothing
Try
oDoc = oDocs.Open(sFile, bConfirmConversions, bReadOnly, bAddToRecentFiles)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
Return oDoc
End Function 'OpenWordDocument method

Public Sub WordSaveAs(oDoc As Object, sFile As String, iFileFormat As Integer) 
Dim bLockComments As Boolean = False
Dim sPassword As String = ""
Dim bAddToRecentFiles As Boolean = False
Try 
oDoc.SaveAs(sFile, iFileFormat, bLockComments, sPassword, bAddToRecentFiles)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' WordSaveAs method

Public Sub WordClose(oDoc As Object) 
Dim oApp As Object = oDoc.Application
ClearNormalTemplate(oApp)

Dim iSaveChanges As Integer = 0
Try 
oDoc.Close(iSaveChanges)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' WordClose method

Public Sub ClearNormalTemplate(oApp As Object) 
Dim oTemplate As Object = oApp.NormalTemplate
oTemplate.Saved = True
Marshal.ReleaseComObject(oTemplate)
oTemplate = Nothing
End Sub ' ClearNormalTemplate method

Public Sub WordQuit(oApp As Object) 
ClearNormalTemplate(oApp)

Dim iSaveChanges As Integer = 0
Try 
oApp.Quit(iSaveChanges)
Catch Ex As COMException
MessageBox.Show(ex.Message, "Error!")
End Try
End Sub ' WordQuit method

Public Function Ppt2Txt(sSource As String, sTarget As String) As Object
Dim iAlerts, iGet, iNote, iNoteLabel, iNoteCount, iOutlineLabel, iShape, iShapeCount, iShip, iShipCount, iSlide, iSlideCount, iType, iVisible As Integer
Dim oApp, oFrame, oNote, oNotes, oPpt, oPpts, oShape, oShapes, oShip, oShips, oSlide, oSlides, oText, oTextEffect, oTextFrame, oTextRange, oType As Object
Dim s, sText As String

Try
oApp = GetObject("PowerPoint.Application")
'iVisible = oApp.visible
iAlerts = oApp.DisplayAlerts
iGet = true
Catch 
oApp = CreateObject("PowerPoint.Application")
iGet = False
End Try
'Must be visible for COM automation to work
oApp.Visible = True
oApp.DisplayAlerts = False
oPpts = oApp.Presentations
'oPpt = oPpts.Open(sSource, -1, 0, 0) ' parameters are ReadOnly, Untitled, WithWindow
'oPpt = oPpts.Open(sSource, True)
oPpt = VB.PowerPointOpen(oPpts, sSource)

' get presentation name and slide count
s = oPpt.Name
sText = Path.GetFileNameWithoutExtension(s)

oSlides = oPpt.Slides
iSlideCount = oSlides.Count
sText = sText +VBCRLF +CStr(iSlideCount) + " Slide" +IIf(iSlideCount = 1, "", "s")
iSlide = 1
While iSlide <=iSlideCount
oSlide = oSlides.Item(iSlide)
sText = sText +VBCRLF + VBCRLF + "----------" + VBCRLF + Chr(12) + VBCRLF +"Slide " +CStr(iSlide)

oNotes = oSlide.NotesPage
iNoteCount = oNotes.Count
iNoteLabel = True
iNote = 1
While iNote <=iNoteCount
oNote = oNotes.Item(iNote)
oShips = oNote.Shapes
iShipCount = oShips.Count
iShip = 1
While iShip <=iShipCount
oShip = oShips.Item(iShip)
If oShip.HasTextFrame Then
oFrame = oShip.TextFrame
oText = oFrame.TextRange
s = oText.Text
If s <>"" Then
If iNoteLabel Then
sText = sText +VBCRLF +"Notes:" +VBCRLF +s
iNoteLabel = False
Else
sText = sText +VBCRLF +s
End If
End If
oText = Nothing
oFrame = Nothing
End If
oShip = Nothing
iShip = iShip +1
End While
oShips = Nothing
oNote = Nothing
sText = sText.Trim()
iNote = iNote +1
End While
oNotes = Nothing

oShapes = oSlide.Shapes
iShapeCount = oShapes.Count
iOutlineLabel = True
iShape = 1
While iShape <=iShapeCount
oShape = oShapes.Item(iShape)
If oShape.HasTextFrame Then
oTextFrame = oShape.TextFrame
oTextRange = oTextFrame.TextRange
s = oTextRange.Text
If s <>"" And s.ToLower() <> "outline" Then
If iOutlineLabel Then
sText = sText +VBCRLF +"Outline:" +VBCRLF +s
iOutlineLabel = False
Else
sText = sText +VBCRLF +s
End If
End If
oTextRange = Nothing
oTextFrame = Nothing
End If
If Not oShape.HasTextFrame Or (oShape.HasTextFrame And s = "") Then
s = oShape.AlternativeText
If s <>"" Then
sText = sText +VBCRLF +s
End If
iType = oShape.Type
If iType = 15 Then ' texteffect
oTexteffect = oShape.TextEffect
s = oTexteffect.Text
If s <>"" And (s <>oShape.alternativetext) Then
sText = sText +VBCRLF +"Text Effect: " +s
End If
oTextEffect = Nothing
End If
oType = Nothing
End If

oShape = Nothing
sText = sText.Trim()
iShape = iShape +1
End While
oShapes = Nothing
oSlide = Nothing
sText = sText.Trim()
iSlide = iSlide +1
End While
oSlides = Nothing
'sText = sText + VBCRLf + "----------" + VBCRLF + "End of Document" + VBCRLF
My.Computer.FileSystem.WriteAllText(sTarget, sText, False)
oPpt.Saved = True
'oPpt.close()
VB.PowerPointClose(oPpt)
Marshal.ReleaseComObject(oPpt)
oPpt = Nothing
Marshal.ReleaseComObject(oPpts)
oPpts = Nothing
If iGet Then
oApp.Visible = iVisible
oApp.DisplayAlerts = iAlerts
Else
'oApp.Quit()
PowerPointQuit(oApp)
End If
Marshal.ReleaseComObject(oApp)
oApp = Nothing
Return File.Exists(sTarget)
End Function ' Ppt2Txt method

Public Function Xls2Txt(sSource As String, sTarget As String) As Boolean
Dim iAlerts, iGet, iSheet, iSheetCount, iUpdating, iVisible As Integer
Dim oApp, oSheet, oSheets, oXls, oXlss As Object
Dim s, sBook, sName As String

Try
oApp = GetObject("Excel.Application")
iGet = True
iVisible = oApp.visible
iAlerts = oApp.DisplayAlerts
iUpdating = oApp.ScreenUpdating
Catch
oApp = CreateObject("Excel.Application")
iGet = False
End Try
oApp.Visible = False
oApp.DisplayAlerts = False
oApp.ScreenUpdating = False
oXlss = oApp.Workbooks
'oXls = oXlss.Open(sSource, 0, -1) ' parameters are UpdateLinks, ReadOnly
oXls = ExcelOpen(oXlss, sSource)
oSheets = oXls.Sheets
iSheetCount = oSheets.Count
iSheet = 1
sBook = ""
While iSheet <=iSheetCount
oSheet = oSheets.Item(iSheet)
if File.Exists(sTarget) Then File.Delete(sTarget)
'oSheet.SaveAs(sTarget, 21) ' parameters are format
ExcelSaveAs(oXls, sTarget, 21)
sName = "Sheet " + iSheet.ToString()
If oSheet.Name.Length > 0 Then sName = sName + ": " + oSheet.Name
oSheet = Nothing
oSheets = Nothing
'oXls.Close(0) ' parameters are SaveChanges
ExcelClose(oXls)
oXls = Nothing
s = sName + VBCRLf + My.Computer.FileSystem.ReadAllText(sTarget).Trim()
sBook = sBook + IIf(iSheet >1, VBCRLF + "----------" + VBCRLF + Chr(12) + VBCRLF, "") +s
'oXls = oXlss.Open(sSource, 0, -1) ' parameters are UpdateLinks, ReadOnly
oXls = VB.ExcelOpen(oXlss, sSource)
oSheets = oXls.Sheets
iSheet = iSheet +1
End While
if File.Exists(sTarget) Then File.Delete(sTarget)
'sBook = sBook.Replace(VBCRLF + VBCRLF, VBCRLF)
'sBook = sBook.Replace(VBCRLF + VBCRLF, VBCRLF)
'sBook = sBook + VBCRLF + "----------" + VBCRLF + "End of Document" + VBCRLF
My.Computer.FileSystem.WriteAllText(sTarget, sBook, False)
oSheet = Nothing
oSheets = Nothing
'oXls.Close(0) ' parameters are SaveChanges
VB.ExcelClose(oXls)
Marshal.ReleaseComObject(oXls)
oXls = Nothing
Marshal.ReleaseComObject(oXlss)
oXlss = Nothing
If iGet Then
oApp.Visible = iVisible
oApp.DisplayAlerts = iAlerts
oApp.ScreenUpdating = iUpdating
Else
'oApp.Quit()
VB.ExcelQuit(oApp)
End If

Marshal.ReleaseComObject(oApp)
oApp = Nothing

Return File.Exists(sTarget)
End Function ' Xls2Txt method

Sub Main()
Dim sSource As String = "C:\temp\temp.ppt"
Dim sTarget As String = "C:\temp\tmp.tmp"
'Xls2Txt(sSource, sTarget)
Ppt2Txt(sSource, sTarget)
End Sub

Function LookupTerm(sWord As String) As String
' Thanks to Pranav Lal for idea of getting table data from dictionary.com

If sWord.Length = 0 Then Return ""
Dim oIE, oDoc, oBody, oRange, oTables, oTable As Object
Dim sUrl, sText, sDivider, sFile As String

'bAppend = True
sUrl = "http://dictionary.reference.com/browse/"
sDivider = VBCRLF + "----------" + VBCRLF + Chr(12) + VBCRLF
'sDivider = ""
sFile = "output.txt"

'sWord = Interaction.Command().Trim()
'Microsoft.VisualBasic.Interaction.MsgBox(sWord)
'If sWord.Length = 0 Then sWord = Interaction.InputBox("Word:", "Dictionary Lookup").Trim()

'Console.WriteLine("Connecting")

sText = sWord + VBCRLF + VBCRLF + "Contents" + VBCRLF
sText = sText + "dictionary.com" + VBCRLF + "thesaurus.com" + VBCRLF + "wikipedia.org"

oIE = Interaction.CreateObject("InternetExplorer.Application")
sUrl = System.Uri.EscapeUriString(sUrl + sWord)
oIE.Navigate(sUrl)
While oIE.ReadyState <> 4
System.Threading.Thread.Sleep(100)
End While

oDoc = oIE.Document
oTables = oDoc.GetElementsByTagName("table")

'If My.Computer.FileSystem.FileExists(sFile) Then sText = sDivider + sText

sText = sText + sDivider + "dictionary.com" + VBCRLF + VBCRLF
For Each oTable In oTables
sText = sText + oTable.InnerText + VBCRLF + VBCRLF
Next
sText = sText.Trim() + VBCRLF
'My.Computer.FileSystem.WriteAllText(sFile, sText, bAppend)

oDoc.Close()

sUrl = "http://thesaurus.reference.com/browse/"
sUrl = System.Uri.EscapeUriString(sUrl + sWord)
oIE.Navigate(sUrl)
While oIE.ReadyState <> 4
System.Threading.Thread.Sleep(100)
End While

oDoc = oIE.Document
oTables = oDoc.GetElementsByTagName("table")

sText = sText + sDivider + "thesaurus.com" + VBCRLF + VBCRLF
For Each oTable In oTables
sText = sText + oTable.InnerText + VBCRLF + VBCRLF
Next
sText = sText.Replace("  Roget's New Millennium™ Thesaurus - Cite This Source" + VBCRLF, "")
sText = sText.Trim()
oDoc.Close()

sUrl = "http://en.wikipedia.org/w/index.php?title="
sUrl = System.Uri.EscapeUriString(sUrl + sWord)
sUrl = sUrl + "&printable=yes"
oIE.Navigate(sUrl)
While oIE.ReadyState <> 4
System.Threading.Thread.Sleep(100)
End While

oDoc = oIE.Document
oBody = oDoc.Body
oRange = oBody.CreateTextRange()
sText = sText + sDivider + "wikipedia.org" + VBCRLF + VBCRLF
sText = sText + oRange.Text
sText = sText.Replace("• Ten things you didn't know about images on Wikipedia •Jump to: navigation, search" + VBCRLF, "")
sText = sText.Trim()
sText = sText + VBCRLF + "----------" + VBCRLF + "End of Document" + VBCRLF

oDoc.Close()
oIE.Quit()
System.Runtime.InteropServices.Marshal.ReleaseComObject(oIE)
oIE = Nothing

return sText
End Function

Function Wikipedia (sWord As String) As String

If sWord.Length = 0 Then Return ""
Dim oIE, oDoc, oBody, oRange As Object
Dim sUrl, sText, sDivider, sFile As String

'bAppend = True
'sUrl = "http://dictionary.reference.com/browse/"
sUrl = "http://en.wikipedia.org/w/index.php?title="
sUrl = sUrl + sWord + "&printable=yes"

'sDivider = VBCRLF + "----------" + VBCRLF + Chr(12) + VBCRLF
sDivider = ""
sFile = "output.txt"

'sWord = Interaction.Command().Trim()
'Microsoft.VisualBasic.Interaction.MsgBox(sWord)
'If sWord.Length = 0 Then sWord = Interaction.InputBox("Word:", "Dictionary Lookup").Trim()

'Console.WriteLine("Connecting")
oIE = Interaction.CreateObject("InternetExplorer.Application")
'System.Windows.Forms.MessageBox.Show(sUrl)
oIE.Navigate(sUrl)
While oIE.ReadyState <> 4
System.Threading.Thread.Sleep(100)
End While

oDoc = oIE.Document
oBody = oDoc.Body
oRange = oBody.CreateTextRange()
sText = oRange.Text
sText = sText.Trim() + VBCRLF
'My.Computer.FileSystem.WriteAllText(sFile, sText, bAppend)

oDoc.Close()
oIE.Quit()
System.Runtime.InteropServices.Marshal.ReleaseComObject(oIE)
oIE = Nothing

return sText
End Function

Function GetLinks(sUrl As String) As List (Of String())

Dim listLinks As List (Of String()) = new List (of String())
Dim listRefs As List (Of String) = new List (of String)
Dim oIE, oDoc, oLinks, oLink As Object

oIE = Interaction.CreateObject("InternetExplorer.Application")
oIE.Navigate(sUrl)
While oIE.ReadyState <> 4
System.Threading.Thread.Sleep(100)
End While

oDoc = oIE.Document
oLinks = oDoc.Links

For Each oLink In oLinks
Dim sRef As String = oLink.HRef
'if sRef.IndexOf("@") >= 0 Then Continue For
if sRef.ToLower().StartsWith("mailto:") Then Continue For
Try 
Dim oUri As Uri = new Uri(sRef)
Catch
sRef = ""
End Try

if sRef.Length = 0 Or listRefs.Contains(sRef) Then Continue For
listRefs.Add(sRef)
Dim aLink() As String = {sRef, oLink.InnerText}
listLinks.Add(aLink)
Next

oDoc.Close()
oIE.Quit()
System.Runtime.InteropServices.Marshal.ReleaseComObject(oIE)
oIE = Nothing

return listLinks
End Function

Public Sub UploadFile(sFile As String, sURL As String, sUserName As String, sPassWord As String)
Dim bShowUI As Boolean = True
'Dim iTimeOut as Integer = new FileInfo(sFile).Length + 1000
Dim iTimeOut as Integer = 100000
Dim CancelOption As Microsoft.VisualBasic.FileIO.UICancelOption = Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException
'Dim CancelOption As Microsoft.VisualBasic.FileIO.UICancelOption = Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing
My.Computer.Network.UploadFile(sFile, sURL, sUserName, sPassword, bShowUI, iTimeOut, CancelOption)
'My.Computer.Network.UploadFile(sFile, sURL, sUserName, sPassword)
End Sub ' UploadFile method

Public Sub DownloadFile(sURL As String, sFile As String, sUserName As String, sPassWord As String)
Dim bShowUI As Boolean = True
' Dim iTimeOut as Integer = 500000
' Dim iTimeOut as Integer = -1 'infinite
' Dim iTimeOut as Integer = 0 'infinite
Dim iTimeOut as Integer = 6000000 
Dim bReplace As Boolean = True
'Dim CancelOption As Microsoft.VisualBasic.FileIO.UICancelOption = Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing
Dim CancelOption As Microsoft.VisualBasic.FileIO.UICancelOption = Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException
'My.Computer.Network.DownloadFile(sURL, sFile, sUserName, sPassword)
My.Computer.Network.DownloadFile(sURL, sFile, sUserName, sPassword, bShowUI, iTimeOut, bReplace, CancelOption)
End Sub ' DownloadFile method

Public sub PlaySystemSound(sound As SystemSound)
My.Computer.Audio.PlaySystemSound(sound)
End Sub

Public sub PlayWav(sWav As String)
'My.Computer.Audio.Play(sWav, AudioPlayMode.BackgroundLoop)
My.Computer.Audio.Play(sWav, AudioPlayMode.WaitToComplete)
End Sub

Public sub StopWav()
My.Computer.Audio.Stop()
End Sub

Public Sub Encrypt2File( sText As String, sFile As String)
Dim DataStream As MemoryStream = New MemoryStream( )
Dim Writer As New StreamWriter(DataStream)
Writer.Write(sText)
Writer.Close( )

Dim Data() As Byte = DataStream.ToArray( )
Dim EncodedData() As Byte = ProtectedData.Protect(Data, Nothing, DataProtectionScope.CurrentUser)
My.Computer.FileSystem.WriteAllBytes(sFile, EncodedData, False)
End Sub

Public Function File2Decrypt(sFile As String) As String
Dim EncodedData() As Byte = My.Computer.FileSystem.ReadAllBytes(sFile)
Dim Data() As Byte = ProtectedData.Unprotect(EncodedData, Nothing, DataProtectionScope.CurrentUser)
Dim DataStream As MemoryStream = New MemoryStream(Data)
Dim Reader As New StreamReader(DataStream)
Dim sText As String = Reader.ReadToEnd()
Reader.Close( )
Return sText
End Function

Sub SendMail(sUserName As String, sPassword As String, sSenderAddress As String, sOutgoingServer As String, sSubject As String, sText As String, sRecipient As String)
Dim cdoSendUsingPickup As Integer = 1 'Send message using the local SMTP service pickup directory.
Dim cdoSendUsingPort As Integer = 2 'Send the message using the network (SMTP over the network).
Dim cdoAnonymous As Integer = 0 'Do not authenticate
Dim cdoBasic As Integer = 1 'basic (clear-text) authentication
Dim cdoNTLM As Integer = 2 'NTLM

Dim oMessage As Object
oMessage = CreateObject("CDO.Message")
'oMessage.Subject = "VBNET Example CDO Message"
oMessage.Subject = sSubject
'oMessage.From = """Jamal Mazrui"" <jamal.mazrui@verizon.net>"
oMessage.From = sSenderAddress
'oMessage.To = "jamal@empowermentzone.com"
oMessage.To = sRecipient
'oMessage.TextBody = "This is some sample message text.." & vbCRLF & "It was sent using SMTP authentication."
oMessage.TextBody = sText

'==This section provides the configuration information for the remote SMTP server.

oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2

'Name or IP of Remote SMTP Server
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpserver") = sOutgoingServer '"outgoing.verizon.net"

'Type of authentication, NONE, Basic (Base64 encoded), NTLM
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = cdoBasic

'Your UserID on the SMTP server
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendusername") = sUserName '"jamal.mazrui"

'Your password on the SMTP server
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendpassword") = sPassword '"jambo6"

'Server port (typically 25)
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25

'Use SSL for the connection (False or True)
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = False

'Connection Timeout in seconds (the maximum time CDO will try to establish a connection to the SMTP server)
oMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 60

oMessage.Configuration.Fields.Update

'==End remote SMTP server configuration section==

oMessage.Send
End Sub

End Module
