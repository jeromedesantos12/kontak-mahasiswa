Imports System.Data.Odbc

Public Class Form3

    Dim Conn As OdbcConnection
    Dim Cmd As OdbcCommand
    Dim Ds As DataSet
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String

    Sub Koneksi()
        MyDB = "Driver={MySQL ODBC 3.51 Driver};Database=db_kampus;Server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        TextBox1.MaxLength = 6
        TextBox2.MaxLength = 50
        TextBox3.MaxLength = 100
        TextBox4.MaxLength = 20
        Button1.Text = "INPUT"
        Button2.Text = "EDIT"
        Button3.Text = "HAPUS"
        Button4.Text = "TUTUP"
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        Call Koneksi()
        Da = New OdbcDataAdapter("Select * From tbl_mahasiswa", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_mahasiswa")
        DataGridView1.DataSource = Ds.Tables("tbl_mahasiswa")
    End Sub

    Sub FieldAktif()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "INPUT" Then
            Button1.Text = "SIMPAN"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "BATAL"
            Call FieldAktif()
        Else
            Call Koneksi()
            Cmd = New OdbcCommand("Select * from tbl_mahasiswa where nim ='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()

            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Pastikan semua field terisi")

            ElseIf Rd.HasRows Then
                MsgBox("NIM tidak boleh sama")
                TextBox1.Text = ""
                TextBox1.Focus()

            Else
                Call Koneksi()
                Dim InputData As String = "Insert into tbl_mahasiswa value ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "')"
                Cmd = New OdbcCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Input Data berhasil")
                Call KondisiAwal()
            End If

        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Button2.Text = "EDIT" Then
            Button2.Text = "SIMPAN"
            Button1.Enabled = False
            Button4.Text = "BATAL"
            Call FieldAktif()
        Else
            Call Koneksi()
            Cmd = New OdbcCommand("Select * from tbl_mahasiswa where nim ='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()

            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Pastikan semua field terisi")

            ElseIf Not Rd.HasRows Then
                MsgBox("NIM belum ada")
                TextBox1.Text = ""
                TextBox1.Focus()

            Else
                Call Koneksi()
                Dim EditData As String = "Update tbl_mahasiswa set namamahasiswa='" & TextBox2.Text & "', alamatmahasiswa='" & TextBox3.Text & "', telpmahasiswa='" & TextBox4.Text & "' where nim='" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(EditData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Edit Data berhasil")
                Call KondisiAwal()
            End If

        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New OdbcCommand("Select * from tbl_mahasiswa where nim ='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Rd.HasRows Then
                TextBox2.Text = Rd.Item("Namamahasiswa")
                TextBox3.Text = Rd.Item("Alamatmahasiswa")
                TextBox4.Text = Rd.Item("telpmahasiswa")
            Else
                MsgBox("Data Tidak Ada")
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Call Koneksi()
        Cmd = New OdbcCommand("Select * from tbl_mahasiswa where nim ='" & TextBox1.Text & "'", Conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()

        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Pastikan data yang akan dihapus terisi")

        ElseIf Not Rd.HasRows Then
            MsgBox("NIM belum ada")
            TextBox1.Text = ""
            TextBox1.Focus()

        Else
            Call Koneksi()
            Dim HapusData As String = "delete from tbl_mahasiswa where nim='" & TextBox1.Text & "'"
            Cmd = New OdbcCommand(HapusData, Conn)
            Cmd.ExecuteNonQuery()
            MsgBox("Hapus Data berhasil")
            Call KondisiAwal()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Button4.Text = "BATAL" Then
            Call KondisiAwal()
        Else
            Me.Close()
        End If
    End Sub

End Class