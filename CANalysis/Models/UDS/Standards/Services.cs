using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Models.UDS.Standards
{
    //  https://en.wikipedia.org/wiki/Unified_Diagnostic_Services#Services
    public enum FunctionGroup
    {
        Diagnostic_and_Communications_Management,
        Data_Transmission,
        Stored_Data_Transmission,
        Input_Output_Control,
        Remote_Activation_of_Routine,
        Upload_Download,
        Negative_Response
    };

    public enum Services
    {
        //  Diagnostic and Communications Management
        Diagnostic_Session_Control = 0x10,
        ECU_Reset = 0x11,
        Clear_DTC = 0x14,
        Security_Access = 0x27,
        Communication_Control = 0x28,
        Authentication = 0x29,
        Tester_Present = 0x3E,
        Access_Timing_Parameters = 0x83,
        Secured_Data_Transmission = 0x84,
        Control_DTC_Settings = 0x85,
        Response_On_Event = 0x86,
        Link_Control = 0x87,

        // Data Transmission
        Read_Data_By_Identifier = 0x22,
        Read_Memory_By_Address = 0x23,
        Read_Scaling_Data_By_ID = 0x24,
        Read_Data_By_ID_Periodic = 0x2A,
        Dynamically_Define_Data_Identifier = 0x2C,
        Write_Data_By_Identifier = 0x2E,
        Write_Memory_By_Address = 0x3D,

        // Stored Data Transmission
        Read_DTC_Information = 0x19,

        //  Input Output Control
        Input_Output_Control_By_Identifier = 0x2F,

        //  Remote Activation of Routine
        Routine_Control = 0x31,

        //  Upload Download
        Request_Download_To_ECU = 0x34,
        Request_Upload_From_ECU = 0x35,
        Transfer_Data = 0x36,
        Request_Transfer_Exit = 0x37,
        Request_File_Transfer = 0x38,

        //  Negative Response
        Negative_Response = 0x7F
    };
}
