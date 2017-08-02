var LoginSAP = {

    setmatricule: function (mat) {
        if (mat == undefined) {
            var userName = prompt("Entrer votre matricule SAP", "");
            //Session("matricule") =  userName ; 
            //var myHdnVar = document.getElementById("Label_LOG_SAP");
            //alert(myHdnVar.innerText);
            //myHdnVar.innerText = userName;
            //alert(myHdnVar.innerText);
            //alert(userName);
            //alert(mat);
            //__doPostBack('LOG_SAP', userName)
            //alert(userName);
            //var file = new ActiveXObject("Scripting.FileSystemObject");
            //var a = file.CreateTextFile("c:\\testfile.txt", true);
            //a.WriteLine(userName);
            //a.Close();
            }
    }


}
$.Notify({caption: 'Button2_Click',content: '1',type: 'success',keepOpen: true});