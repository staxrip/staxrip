
Imports StaxRip.UI

Public Class EventCommandEditor
    Inherits DialogBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents cbEvent As System.Windows.Forms.ComboBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents CriteriaControl As StaxRip.UI.CriteriaControl
    Friend WithEvents tbCommand As System.Windows.Forms.TextBox
    Friend WithEvents bnCommand As StaxRip.UI.ButtonEx
    Friend WithEvents cmsCommands As ContextMenuStripEx
    Friend WithEvents pgParameters As PropertyGridEx
    Friend WithEvents lParameters As System.Windows.Forms.Label
    Friend WithEvents rbMatchAllCriteria As System.Windows.Forms.RadioButton
    Friend WithEvents rbMatchAnyCriteria As System.Windows.Forms.RadioButton
    Friend WithEvents gbCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents gbCommand As System.Windows.Forms.GroupBox
    Friend WithEvents gbName As System.Windows.Forms.GroupBox
    Friend WithEvents gbEvent As System.Windows.Forms.GroupBox
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents tlpCommand As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.cbEvent = New System.Windows.Forms.ComboBox()
        Me.tbCommand = New System.Windows.Forms.TextBox()
        Me.bnCommand = New StaxRip.UI.ButtonEx()
        Me.cmsCommands = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.CriteriaControl = New StaxRip.UI.CriteriaControl()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.rbMatchAllCriteria = New System.Windows.Forms.RadioButton()
        Me.rbMatchAnyCriteria = New System.Windows.Forms.RadioButton()
        Me.pgParameters = New StaxRip.UI.PropertyGridEx()
        Me.lParameters = New System.Windows.Forms.Label()
        Me.gbCriteria = New System.Windows.Forms.GroupBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.gbCommand = New System.Windows.Forms.GroupBox()
        Me.tlpCommand = New System.Windows.Forms.TableLayoutPanel()
        Me.gbName = New System.Windows.Forms.GroupBox()
        Me.gbEvent = New System.Windows.Forms.GroupBox()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbCriteria.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.gbCommand.SuspendLayout()
        Me.tlpCommand.SuspendLayout()
        Me.gbName.SuspendLayout()
        Me.gbEvent.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbName
        '
        Me.tbName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbName.Location = New System.Drawing.Point(15, 63)
        Me.tbName.Margin = New System.Windows.Forms.Padding(10)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(496, 55)
        Me.tbName.TabIndex = 0
        '
        'cbEvent
        '
        Me.cbEvent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEvent.FormattingEnabled = True
        Me.cbEvent.Location = New System.Drawing.Point(15, 63)
        Me.cbEvent.Margin = New System.Windows.Forms.Padding(10)
        Me.cbEvent.Name = "cbEvent"
        Me.cbEvent.Size = New System.Drawing.Size(496, 56)
        Me.cbEvent.Sorted = True
        Me.cbEvent.TabIndex = 0
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCommand.Location = New System.Drawing.Point(0, 7)
        Me.tbCommand.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.ReadOnly = True
        Me.tbCommand.Size = New System.Drawing.Size(954, 55)
        Me.tbCommand.TabIndex = 0
        '
        'bnCommand
        '
        Me.bnCommand.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCommand.ContextMenuStrip = Me.cmsCommands
        Me.bnCommand.Location = New System.Drawing.Point(964, 0)
        Me.bnCommand.Margin = New System.Windows.Forms.Padding(0)
        Me.bnCommand.ShowMenuSymbol = True
        Me.bnCommand.Size = New System.Drawing.Size(70, 70)
        '
        'cmsCommands
        '
        Me.cmsCommands.ImageScalingSize = New System.Drawing.Size(29, 29)
        Me.cmsCommands.Name = "cmsCommands"
        Me.cmsCommands.Size = New System.Drawing.Size(61, 4)
        '
        'CriteriaControl
        '
        Me.CriteriaControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CriteriaControl.BackColor = System.Drawing.SystemColors.Window
        Me.CriteriaControl.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.CriteriaControl.Location = New System.Drawing.Point(16, 137)
        Me.CriteriaControl.Margin = New System.Windows.Forms.Padding(10)
        Me.CriteriaControl.Name = "CriteriaControl"
        Me.CriteriaControl.Size = New System.Drawing.Size(1034, 56)
        Me.CriteriaControl.TabIndex = 1
        '
        'rbMatchAllCriteria
        '
        Me.rbMatchAllCriteria.Location = New System.Drawing.Point(5, 5)
        Me.rbMatchAllCriteria.Margin = New System.Windows.Forms.Padding(5)
        Me.rbMatchAllCriteria.Name = "rbMatchAllCriteria"
        Me.rbMatchAllCriteria.Size = New System.Drawing.Size(253, 70)
        Me.rbMatchAllCriteria.TabIndex = 0
        Me.rbMatchAllCriteria.TabStop = True
        Me.rbMatchAllCriteria.Text = "Match All"
        Me.TipProvider.SetTipText(Me.rbMatchAllCriteria, "Command gets only executed if all criteria is true.")
        Me.rbMatchAllCriteria.UseVisualStyleBackColor = True
        '
        'rbMatchAnyCriteria
        '
        Me.rbMatchAnyCriteria.Location = New System.Drawing.Point(268, 5)
        Me.rbMatchAnyCriteria.Margin = New System.Windows.Forms.Padding(5)
        Me.rbMatchAnyCriteria.Name = "rbMatchAnyCriteria"
        Me.rbMatchAnyCriteria.Size = New System.Drawing.Size(273, 70)
        Me.rbMatchAnyCriteria.TabIndex = 2
        Me.rbMatchAnyCriteria.TabStop = True
        Me.rbMatchAnyCriteria.Text = "Match Any"
        Me.TipProvider.SetTipText(Me.rbMatchAnyCriteria, "Command gets executed if any criteria is true.")
        Me.rbMatchAnyCriteria.UseVisualStyleBackColor = True
        '
        'pgParameters
        '
        Me.pgParameters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgParameters.HelpVisible = False
        Me.pgParameters.LineColor = System.Drawing.SystemColors.ControlDark
        Me.pgParameters.Location = New System.Drawing.Point(14, 166)
        Me.pgParameters.Margin = New System.Windows.Forms.Padding(7)
        Me.pgParameters.Name = "pgParameters"
        Me.pgParameters.PropertySort = System.Windows.Forms.PropertySort.Alphabetical
        Me.pgParameters.Size = New System.Drawing.Size(1036, 44)
        Me.pgParameters.TabIndex = 2
        Me.pgParameters.ToolbarVisible = False
        '
        'lParameters
        '
        Me.lParameters.AutoSize = True
        Me.lParameters.Location = New System.Drawing.Point(15, 117)
        Me.lParameters.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lParameters.Name = "lParameters"
        Me.lParameters.Size = New System.Drawing.Size(205, 48)
        Me.lParameters.TabIndex = 1
        Me.lParameters.Text = "Parameters:"
        '
        'gbCriteria
        '
        Me.gbCriteria.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbCriteria, 2)
        Me.gbCriteria.Controls.Add(Me.FlowLayoutPanel1)
        Me.gbCriteria.Controls.Add(Me.CriteriaControl)
        Me.gbCriteria.Location = New System.Drawing.Point(10, 170)
        Me.gbCriteria.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbCriteria.Name = "gbCriteria"
        Me.gbCriteria.Padding = New System.Windows.Forms.Padding(7)
        Me.gbCriteria.Size = New System.Drawing.Size(1064, 207)
        Me.gbCriteria.TabIndex = 2
        Me.gbCriteria.TabStop = False
        Me.gbCriteria.Text = "Criteria"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.rbMatchAllCriteria)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbMatchAnyCriteria)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnAdd)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(14, 50)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(5)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(756, 80)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'bnAdd
        '
        Me.bnAdd.Location = New System.Drawing.Point(551, 5)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(5)
        Me.bnAdd.Size = New System.Drawing.Size(200, 70)
        Me.bnAdd.Text = "Add"
        '
        'gbCommand
        '
        Me.gbCommand.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbCommand, 2)
        Me.gbCommand.Controls.Add(Me.tlpCommand)
        Me.gbCommand.Controls.Add(Me.pgParameters)
        Me.gbCommand.Controls.Add(Me.lParameters)
        Me.gbCommand.Location = New System.Drawing.Point(10, 377)
        Me.gbCommand.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbCommand.Name = "gbCommand"
        Me.gbCommand.Padding = New System.Windows.Forms.Padding(7)
        Me.gbCommand.Size = New System.Drawing.Size(1064, 224)
        Me.gbCommand.TabIndex = 4
        Me.gbCommand.TabStop = False
        Me.gbCommand.Text = "Command"
        '
        'tlpCommand
        '
        Me.tlpCommand.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpCommand.ColumnCount = 2
        Me.tlpCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpCommand.Controls.Add(Me.bnCommand, 1, 0)
        Me.tlpCommand.Controls.Add(Me.tbCommand, 0, 0)
        Me.tlpCommand.Location = New System.Drawing.Point(16, 49)
        Me.tlpCommand.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.tlpCommand.Name = "tlpCommand"
        Me.tlpCommand.RowCount = 1
        Me.tlpCommand.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCommand.Size = New System.Drawing.Size(1034, 70)
        Me.tlpCommand.TabIndex = 4
        '
        'gbName
        '
        Me.gbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbName.Controls.Add(Me.tbName)
        Me.gbName.Location = New System.Drawing.Point(10, 10)
        Me.gbName.Margin = New System.Windows.Forms.Padding(10, 10, 6, 10)
        Me.gbName.Name = "gbName"
        Me.gbName.Padding = New System.Windows.Forms.Padding(15)
        Me.gbName.Size = New System.Drawing.Size(526, 150)
        Me.gbName.TabIndex = 1
        Me.gbName.TabStop = False
        Me.gbName.Text = "Name"
        '
        'gbEvent
        '
        Me.gbEvent.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbEvent.Controls.Add(Me.cbEvent)
        Me.gbEvent.Location = New System.Drawing.Point(548, 10)
        Me.gbEvent.Margin = New System.Windows.Forms.Padding(6, 10, 10, 10)
        Me.gbEvent.Name = "gbEvent"
        Me.gbEvent.Padding = New System.Windows.Forms.Padding(15)
        Me.gbEvent.Size = New System.Drawing.Size(526, 150)
        Me.gbEvent.TabIndex = 3
        Me.gbEvent.TabStop = False
        Me.gbEvent.Text = "Event"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(270, 10)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(10)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(10, 10)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(10, 10, 0, 10)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.gbName, 0, 0)
        Me.tlpMain.Controls.Add(Me.gbEvent, 1, 0)
        Me.tlpMain.Controls.Add(Me.gbCriteria, 0, 1)
        Me.tlpMain.Controls.Add(Me.gbCommand, 0, 2)
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel2, 1, 3)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1084, 704)
        Me.tlpMain.TabIndex = 6
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel2.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(554, 607)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0, 6, 0, 6)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(530, 90)
        Me.FlowLayoutPanel2.TabIndex = 9
        '
        'EventCommandEditor
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1084, 704)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "EventCommandEditor"
        Me.Text = "Event Command Editor"
        Me.gbCriteria.ResumeLayout(False)
        Me.gbCriteria.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.gbCommand.ResumeLayout(False)
        Me.gbCommand.PerformLayout()
        Me.tlpCommand.ResumeLayout(False)
        Me.tlpCommand.PerformLayout()
        Me.gbName.ResumeLayout(False)
        Me.gbName.PerformLayout()
        Me.gbEvent.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private GridTypeDescriptor As New GridTypeDescriptor
    Private CommandParameters As CommandParameters
    Private EventCommandValue As EventCommand

    Sub New(ev As EventCommand)
        MyBase.New()
        InitializeComponent()

        ScaleClientSize(37, 30)
        EventCommandValue = ev
        tbName.Text = EventCommandValue.Name

        cbEvent.Items.AddRange(ListBag(Of ApplicationEvent).GetBagsForEnumType)
        ListBag(Of ApplicationEvent).SelectItem(cbEvent, EventCommandValue.Event)

        rbMatchAllCriteria.Checked = Not ev.OrOnly
        rbMatchAnyCriteria.Checked = ev.OrOnly

        Dim allCriteria As New List(Of Criteria)

        For Each m In Macro.GetMacros(False, False, False)
            Dim c = Criteria.Create(m.Type)
            c.Name = m.FriendlyName
            c.Description = m.Description
            c.Macro = m.Name
            allCriteria.Add(c)
        Next

        allCriteria.Sort(New Comparer(Of Criteria)(NameOf(Criteria.Name)))

        CriteriaControl.AllCrieria = allCriteria
        CriteriaControl.CriteriaList = EventCommandValue.CriteriaList

        If Not EventCommandValue.CommandParameters Is Nothing Then
            CommandParameters = DirectCast(ObjectHelp.GetCopy(EventCommandValue.CommandParameters), CommandParameters)
        End If

        SetCommandParameters(CommandParameters)
        Command.PopulateCommandMenu(cmsCommands.Items, New List(Of Command)(g.MainForm.CustomMainMenu.CommandManager.Commands.Values), AddressOf MenuClick)

        For Each i As ToolStripMenuItem In cmsCommands.Items
            If i.Text = "Dynamic" Then
                i.Visible = False
            End If
        Next

        TipProvider.SetTip("Parameters used to execute the command.", pgParameters, lParameters)
        TipProvider.SetTip("Criteria can be defined optionally to execute the command only if the criteria is matched.", CriteriaControl, gbCriteria)
        TipProvider.SetTip("Command to be executed.", tbCommand, gbCommand)
        TipProvider.SetTip("The event on which the command gets executed.", cbEvent, gbEvent)
        TipProvider.SetTip("Name of the Event Command.", tbName, gbName)
    End Sub

    Sub MenuClick(c As Command)
        CommandParameters = New CommandParameters(c.MethodInfo.Name, c.GetDefaultParameters.ToArray)
        SetCommandParameters(CommandParameters)
    End Sub

    Sub SetCommandParameters(cp As CommandParameters)
        If cp Is Nothing Then
            pgParameters.Visible = False
            lParameters.Visible = False
            Exit Sub
        End If

        Dim cm = g.MainForm.CustomMainMenu.CommandManager

        If Not cm.HasCommand(cp.MethodName) Then
            pgParameters.Visible = False
            lParameters.Visible = False
            Exit Sub
        End If

        Dim c = g.MainForm.CustomMainMenu.CommandManager.GetCommand(cp.MethodName)
        tbCommand.Text = cp.MethodName

        If cp.Parameters Is Nothing OrElse cp.Parameters.Count = 0 Then
            pgParameters.Visible = False
            lParameters.Visible = False
            Exit Sub
        End If

        Dim mi = c.MethodInfo

        If mi Is Nothing Then
            pgParameters.Visible = False
            lParameters.Visible = False
            Exit Sub
        End If

        pgParameters.Visible = True
        lParameters.Visible = True

        GridTypeDescriptor.Items.Clear()

        Dim params = mi.GetParameters

        For i = 0 To params.Length - 1
            Dim gp As New GridProperty
            gp.Name = DispNameAttribute.GetValue(params(i).GetCustomAttributes(False))

            If gp.Name Is Nothing Then
                gp.Name = params(i).Name.ToTitleCase
            End If

            gp.Value = c.FixParameters(cp.Parameters)(i)
            gp.Description = DescriptionAttributeHelp.GetDescription(params(i).GetCustomAttributes(False))
            gp.TypeEditor = EditorAttributeHelp.GetEditor(params(i).GetCustomAttributes(False))

            GridTypeDescriptor.Add(gp)
        Next

        pgParameters.SelectedObject = GridTypeDescriptor
        SetSplitter()
    End Sub

    Sub EventCommandEditor_FormClosed() Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            EventCommandValue.Name = tbName.Text
            EventCommandValue.Event = ListBag(Of ApplicationEvent).GetValue(cbEvent)
            EventCommandValue.CriteriaList = CriteriaControl.CriteriaList
            EventCommandValue.CommandParameters = CommandParameters
            EventCommandValue.OrOnly = rbMatchAnyCriteria.Checked
        End If
    End Sub

    Sub EventCommandEditor_HelpRequested() Handles Me.HelpRequested
        g.ShowPage("commands")
    End Sub

    Sub pgParameters_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgParameters.PropertyValueChanged
        CommandParameters.Parameters.Clear()

        For Each i As DictionaryEntry In GridTypeDescriptor.Items
            CommandParameters.Parameters.Add(DirectCast(i.Value, GridProperty).Value)
        Next
    End Sub

    Sub EventCommandEditor_Shown() Handles Me.Shown
        SetSplitter()
    End Sub

    Sub EventCommandEditor_SizeChanged() Handles Me.SizeChanged
        SetSplitter()
    End Sub

    Sub SetSplitter()
        pgParameters.MoveSplitter(pgParameters.Width \ 3)
    End Sub

    Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        CriteriaControl.AddItem(Nothing)
    End Sub
End Class
