using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPBusinessCards.Utils {
    internal static class ConnexionAD {

        const string adDomainName = "eolane.com";
        const string adDefaultOU = "DC=eolane,DC=com";
        const string adUserAccount = "ee_trombi";
        const string adUserAccountPassword = "*Eol49!";

        public static String GetPositionUser(String cnCurrentUser) {
            String PositionUser = "Undefined";
            PrincipalContext ctx = null;
            ctx = new PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword);
            using (UserPrincipal userAd = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, cnCurrentUser)){
                if (userAd != null) {
                    try {
                        DirectoryEntry directoryEntry = userAd.GetUnderlyingObject() as DirectoryEntry;
                        //On récupèrela fonction de l'utilisateur
                        string userAdTitle = directoryEntry.Properties["title"].Value.ToString();

                        if (userAdTitle != null)
                            PositionUser = userAdTitle;
                        else
                            PositionUser = "Undefined";
                    }
                    catch (Exception e){
                        return PositionUser;}
                }
                else
                    PositionUser = "No User";
            }
            return PositionUser;
        }

        public static String getImageUser(String cnCurrentUser) {
            String PictureUser = "<img src='/medias/mediasEolane/userProfileDefault.png' alt='photo' />"; ;
            PrincipalContext ctx = null;

            ctx = new PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword);
            using (UserPrincipal userAd = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, cnCurrentUser)) {
                if (userAd != null) {
                    try
                    {
                        DirectoryEntry directoryEntry = userAd.GetUnderlyingObject() as DirectoryEntry;
                        //On récupère la photo de l'utilisateur
                        byte[] data = directoryEntry.Properties["thumbnailphoto"].Value as byte[];
                        if (data != null)
                            PictureUser = "<img src='data:image/jpeg;base64, " + System.Convert.ToBase64String(data) + "' alt='photo' />";
                        //En attente de la photo (à enlever à terme !!)
                        else if (directoryEntry.Properties["mail"].Value.ToString() == "olivier.bertrand@eolane.com")
                            PictureUser = "<img src='/medias/mediasEolane/olivierb.jpg' alt='photo' />";
                        else
                            //On gère le cas où il n'ya pas de photo
                            PictureUser = "<img src='/medias/mediasEolane/userProfileDefault.png' alt='photo' />";
                    }
                    catch (Exception e){
                        return PictureUser;}
                }
                else
                    PictureUser = "<img src='/medias/mediasEolane/userProfileDefault.png' alt='photo' />";
            }
            return PictureUser;
        }

        public static Dictionary<string, string> GetUserInfos(String cnCurrentUser) {
            PrincipalContext ctx = null;
            Dictionary<string, string> tabInfosUser = new Dictionary<string, string>();
            ctx = new PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword);

            using (UserPrincipal userAd = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, cnCurrentUser)){
                if (userAd != null) {
                    try
                    {
                        tabInfosUser["title"] = null;
                        tabInfosUser["mail"] = null;
                        tabInfosUser["telephonenumber"] = null;
                        tabInfosUser["facsimiletelephonenumber"] = null;
                        tabInfosUser["company"] = null;
                        tabInfosUser["displayname"] = null;

                        DirectoryEntry directoryEntry = userAd.GetUnderlyingObject() as DirectoryEntry;

                        if (directoryEntry.Properties.Contains("title"))
                            tabInfosUser["title"] = directoryEntry.Properties["title"].Value.ToString(); //Job Title
                        if (directoryEntry.Properties.Contains("mail"))
                            tabInfosUser["mail"] = directoryEntry.Properties["mail"].Value.ToString(); //Mail
                        if (directoryEntry.Properties.Contains("telephonenumber"))
                            tabInfosUser["telephonenumber"] = directoryEntry.Properties["telephonenumber"].Value.ToString(); //Phone Number
                        if (directoryEntry.Properties.Contains("facsimiletelephonenumber"))
                            tabInfosUser["facsimiletelephonenumber"] = directoryEntry.Properties["facsimiletelephonenumber"].Value.ToString(); //Fax Number
                        if (directoryEntry.Properties.Contains("company"))
                            tabInfosUser["company"] = directoryEntry.Properties["company"].Value.ToString(); //Company
                        if (directoryEntry.Properties.Contains("displayname"))
                            tabInfosUser["displayname"] = directoryEntry.Properties["displayname"].Value.ToString(); //Company
                    }
                    catch (Exception e){
                        Console.WriteLine("Exception trouvée : " + e);
                        throw; }
                }
                return tabInfosUser;
            }
        }

        public static Dictionary<string, string> GetSpecificUser(string user)
        {
            Dictionary<string, string> specificUser = new Dictionary<string, string>();

            string error = null;
            PrincipalContext ctx = null;
            ctx = new PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword);

            using (UserPrincipal userAd = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, user)){
                if (userAd != null){
                    try {
                        DirectoryEntry directoryEntry = userAd.GetUnderlyingObject() as DirectoryEntry;

                        byte[] dataImg = directoryEntry.Properties["thumbnailphoto"].Value as byte[];
                        string pictureUser = (dataImg != null) ? System.Convert.ToBase64String(dataImg) : null;

                        specificUser["thumbnailphoto"] = pictureUser;
                        specificUser["samaccountname"] = directoryEntry.Properties["samaccountname"].Value as string;
                        specificUser["mail"] = directoryEntry.Properties["mail"].Value as string;
                        specificUser["displayname"] = directoryEntry.Properties["displayname"].Value as string;
                        specificUser["title"] = directoryEntry.Properties["title"].Value as string;
                        specificUser["telephonenumber"] = directoryEntry.Properties["telephonenumber"].Value as string;
                        specificUser["sn"] = directoryEntry.Properties["sn"].Value as string;
                        specificUser["givenname"] = directoryEntry.Properties["givenname"].Value as string;
                        specificUser["facsimiletelephonenumber"] = directoryEntry.Properties["facsimiletelephonenumber"].Value as string;
                        specificUser["company"] = directoryEntry.Properties["company"].Value as string;
                        specificUser["Mobile"] = directoryEntry.Properties["Mobile"].Value as string;
                        specificUser["l"] = directoryEntry.Properties["l"].Value as string;
                        specificUser["st"] = directoryEntry.Properties["st"].Value as string;
                        specificUser["postalcode"] = directoryEntry.Properties["postalcode"].Value as string;
                        specificUser["streetaddress"] = directoryEntry.Properties["streetaddress"].Value as string;
                    }
                    catch (Exception e){
                        //return "Erreur, Exception :" + e;
                        throw;}
                }
            }
            return specificUser;
        }
    }
}
