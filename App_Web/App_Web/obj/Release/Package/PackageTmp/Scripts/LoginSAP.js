var LoginSAP = {

    setmatricule: function (mat) {
        if (mat == undefined) {
            var userName = prompt("Entrer votre matricule SAP", "");
            //Session("matricule") =  userName ; 
            var myHdnVar = document.getElementById("Label_LOG_SAP");
            //alert(myHdnVar.innerText);
            myHdnVar.innerText = userName;
            //alert(myHdnVar.innerText);
            //alert(userName);
            //alert(mat);
            }
    }


}