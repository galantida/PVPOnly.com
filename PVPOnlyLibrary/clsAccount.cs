using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    // to do
    // move passworld out of the properties section and into functions
    public class clsAccount : clsBase, intBase
    {
        private int _UTCOffset, _profileImageFileID, _sponsorID;
        private string _email, _password, _firstName, _lastName, _callSign, _profilePic, _cellNumber, _lastIP, _confirmation, _notes, _timeZone, _description;
        private bool _confirmed, _banned, _admin, _eMailDirect, _eMailBoardActivity, _eMailThreadActivity, _eMailInvitationActivity, _eMailVolunteerActivity, _eMailActionActivity, _eMailInformationActivity, _eMailSystemAnnouncements;
        private DateTime _lastLogin, _dateOfBirth;

        // numeric properties
        public int ProfileImageID
        {
            get
            {
                return _profileImageFileID;
            }
            set
            {
                _profileImageFileID = value;
            }
        }

        public int SponsorID
        {
            get
            {
                return _sponsorID;
            }
            set
            {
                _sponsorID = value;
            }
        }

        public int UTCOffset
        {
            get
            {
                return _UTCOffset;
            }
            set
            {
                _UTCOffset = value;
            }
        }

        // date time properties
        public DateTime LastLogin
        {
            get
            {
                return _lastLogin;
            }
            set
            {
                _lastLogin = value;
            }
        }
        public DateTime DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
            }
        }


        // string properties
        public string eMail
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        public string CallSign
        {
            get
            {
                return _callSign;
            }
            set
            {
                _callSign = value;
            }
        }
        public string ProfilePic
        {
            get
            {
                return _profilePic;
            }
            set
            {
                _profilePic = value;
            }
        }
        public string CellNumber
        {
            get
            {
                return _cellNumber;
            }
            set
            {
                _cellNumber = value;
            }
        }
        public string LastIP
        {
            get
            {
                return _lastIP;
            }
            set
            {
                _lastIP = value;
            }
        }

        public string Confirmation
        {
            get
            {
                return _confirmation;
            }
            set
            {
                _confirmation = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        public string TimeZone
        {
            get
            {
                return _timeZone;
            }
            set
            {
                _timeZone = value;
            }
        }

        // bool properties
        public bool Confirmed
        {
            get
            {
                return _confirmed;
            }
            set
            {
                _confirmed = value;
            }
        }

        public bool Banned
        {
            get
            {
                return _banned;
            }
            set
            {
                _banned = value;
            }
        }

        public bool Admin   
        {
            get
            {
                return _admin;
            }
            set
            {
                _admin = value;
            }
        }

        public bool eMailDirect
        {
            get
            {
                return _eMailDirect;
            }
            set
            {
                _eMailDirect = value;
            }
        }

        public bool eMailBoardActivity
        {
            get
            {
                return _eMailBoardActivity;
            }
            set
            {
                _eMailBoardActivity = value;
            }
        }

        public bool eMailThreadActivity
        {
            get
            {
                return _eMailThreadActivity;
            }
            set
            {
                _eMailThreadActivity = value;
            }
        }

        public bool eMailInvitationActivity
        {
            get
            {
                return _eMailInvitationActivity;
            }
            set
            {
                _eMailInvitationActivity = value;
            }
        }

        public bool eMailVolunteerActivity
        {
            get
            {
                return _eMailVolunteerActivity;
            }
            set
            {
                _eMailVolunteerActivity = value;
            }
        }

        public bool eMailActionActivity
        {
            get
            {
                return _eMailActionActivity;
            }
            set
            {
                _eMailActionActivity = value;
            }
        }
        public bool eMailInformationActivity
        {
            get
            {
                return _eMailInformationActivity;
            }
            set
            {
                _eMailInformationActivity = value;
            }
        }
        public bool eMailSystemAnnouncements
        {
            get
            {
                return _eMailSystemAnnouncements;
            }
            set
            {
                _eMailSystemAnnouncements = value;
            }
        }

        // constructors
        public clsAccount() : base() {}
        public clsAccount(SqlDataReader dr) : base(dr){}
        public clsAccount(int id) : base(id) {}


        // convert a generic object to a typed one
        private static clsAccount get(string sqlQuery)
        {
            return (clsAccount)get(sqlQuery, typeof(clsAccount));
        }


        // convert a generic list to a typed list
        private static List<clsAccount> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsAccount));

            List<clsAccount> results = new List<clsAccount>();
            foreach (object obj in objs)
            {
                clsAccount result = new clsAccount();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        public static clsAccount get(int id)
        {
            return get("SELECT * FROM Accounts WHERE ID = " + id);
        }

        public static clsAccount getByUserName(string userName)
        {
            return get("SELECT * FROM Accounts WHERE eMail = '" + userName + "'");
        }

        public static clsAccount getByConfirmationCode(int code)
        {
            return get("SELECT * FROM Accounts WHERE confirmation = " + code);
        }

        public static List<clsAccount> get()
        {
            return getList("SELECT * FROM Accounts");
        }
    }
}