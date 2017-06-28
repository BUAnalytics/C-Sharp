using System.Collections.Generic;

namespace BUAnalytics{
	
	public enum BUError{

		//! Core
		Unknown = 100,
		Server = 200,
		NotFound = 300,
		Connection = 400,
		Json = 500,

		//! Users
		USR_Invalid = 2000,
		USR_NotFound = 2001,
		USR_Disabled = 2002,
		USR_Access = 2003,

		USR_InvalidLogin = 2100,
		USR_InvalidUsername = 2101,
		USR_InvalidEmail = 2102,
		USR_InvalidPassword = 2103,
		USR_InvalidAdmin = 2104,
		USR_InvalidStatus = 2105,
		USR_InvalidProjects = 2106,
		USR_InvalidProject = 2107,

		USR_IncorrectLogin = 2200,

		USR_ExistingUsername = 2300,
		USR_ExistingEmail = 2301,

		//! Sessions
		SES_Invalid = 3000,
		SES_Incorrect = 3001,
		SES_Disabled = 3002,

		//! Projects
		PRJ_Invalid = 41000,
		PRJ_NotFound = 41001,
		PRJ_Incorrect = 41002,
		PRJ_Access = 41003,

		PRJ_InvalidName = 41100,
		PRJ_InvalidVisible = 41101,
		PRJ_InvalidSubtitle = 41102,
		PRJ_InvalidDescription = 41103,
		PRJ_InvalidBody = 41104,
		PRJ_InvalidIcon = 41105,
		PRJ_InvalidImage = 41106,

		//! Access Keys
		PRJ_ACK_Invalid = 42000,
		PRJ_ACK_NotFound = 42001,
		PRJ_ACK_Incorrect = 42002,

		PRJ_ACK_InvalidName = 42100,
		PRJ_ACK_InvalidStatus = 42101,

		//! Collections
		PRJ_COL_Invalid = 43000,
		PRJ_COL_NotFound = 43001,
		PRJ_COL_Access = 43002,

		PRJ_COL_InvalidName = 43100,
		PRJ_COL_InvalidBody = 43101
	}

}