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
		ACK_Invalid = 42000,
		ACK_NotFound = 42001,
		ACK_Incorrect = 42002,

		ACK_InvalidName = 42100,
		ACK_InvalidStatus = 42101,

		//! Collections
		COL_Invalid = 43000,
		COL_NotFound = 43001,
		COL_Access = 43002,

		COL_InvalidSize = 43100,
		COL_InvalidPage = 43101,
		COL_InvalidSort = 43102,
		COL_InvalidDir = 43103,
		COL_InvalidName = 43104,
		COL_InvalidBody = 43105,

		//! Visuals
		VIS_Invalid = 44000,
		VIS_NotFound = 44001,
		VIS_Access = 44002,

		VIS_InvalidCollection = 44100,
		VIS_InvalidName = 44101,
		VIS_InvalidType = 44102,
		VIS_InvalidOptions = 44103,

		VIS_IncorrectCollection = 44200,
		VIS_IncorrectType = 44201,

		//! Applications
		APP_MissingContName = 51000,
		APP_MissingContEmail = 51001,
		APP_MissingContPhone = 51002,
		APP_MissingOrgName = 51003,
		APP_MissingOrgAddress = 51004,
		APP_MissingProjDuration = 51005,
		APP_MissingProjPurpose = 51006,
		APP_MissingProjDescription = 51007
	}

}