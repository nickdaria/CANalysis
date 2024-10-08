﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Models.UDS.Standards
{
    //  https://en.wikipedia.org/wiki/Unified_Diagnostic_Services#Negative_response_codes
    public enum NegativeResponseCode
    {
        General_Reject = 0x10,
        Service_Not_Supported = 0x11,
        Subfunction_Not_Supported = 0x12,
        Incorrect_Message_Length_Or_Invalid_Format = 0x13,
        Response_Too_Long = 0x14,
        Busy_Repeat_Request = 0x21,
        Conditions_Not_Correct = 0x22,
        Request_Sequence_Error = 0x24,
        No_Response_From_Subnet_Component = 0x25,
        Failure_Prevents_Exection_Of_Requested_Action = 0x26,
        Request_Out_Of_Range = 0x31,
        Security_Access_Denied = 0x33,
        Authentication_Failed = 0x34,
        Invalid_Key = 0x35,
        Exceed_Number_Of_Attempts = 0x36,
        Required_Time_Delay_Not_Expired = 0x37,
        Secure_Data_Transmission_Required = 0x38,
        Secure_Data_Transmission_Not_Allowed = 0x39,
        Secure_Data_Verification_Failed = 0x3A,
        Certificate_Validation_Failed_Invalid_Time_Period = 0x50,
        Certificate_Validation_Failed_Invalid_Signature = 0x51,
        Certificate_Validation_Failed_Invalid_Chain_Of_Trust = 0x52,
        Certificate_Validation_Failed_Invalid_Certificate_Type = 0x53,
        Certificate_Validation_Failed_Invalid_Format = 0x54,
        Certificate_Validation_Failed_Invalid_Content = 0x55,
        Certificate_Validation_Failed_Invalid_Scope = 0x56,
        Certificate_Validation_Failed_Invalid_Certificate = 0x57,
        Ownership_Verification_Failed = 0x58,
        Challenge_Calculation_Failed = 0x59,
        Setting_Access_Right_Failed = 0x5A,
        Session_Key_Creation_Derivation_Failed = 0x5B,
        Configuration_Data_Usage_Failed = 0x5C,
        Deauthentication_Failed = 0x5D,
        Upload_Download_Not_Accepted = 0x70,
        Transfer_Data_Suspended = 0x71,
        General_Programming_Failure = 0x72,
        Wrong_Block_Sequence_Number = 0x73,
        Request_Correctly_Received_Response_Pending = 0x78,
        Subfunction_Not_Supported_In_Active_Session = 0x7E,
        Service_Not_Supported_In_Active_Session = 0x7F,
        RPM_Too_High = 0x81,
        RPM_Too_Low = 0x82,
        Engine_Is_Running = 0x83,
        Engine_Is_Not_Running = 0x84,
        Engine_Run_Time_Too_Low = 0x85,
        Temperature_Too_High = 0x86,
        Temperature_Too_Low = 0x87,
        Vehicle_Speed_Too_High = 0x88,
        Vehicle_Speed_Too_Low = 0x89,
        Throttle_Pedal_Too_High = 0x8A,
        Throttle_Pedal_Too_Low = 0x8B,
        Transmission_Range_Not_In_Neutral = 0x8C,
        Transmission_Range_Not_In_Gear = 0x8D,
        Brake_Switch_Not_Closed = 0x8F,
        Shifter_Lever_Not_In_Park = 0x90,
        Torque_Converter_Clutch_Locked = 0x91,
        Voltage_Too_High = 0x92,
        Voltage_Too_Low = 0x93,
        Resource_Temporarily_Unavailable = 0x94,
    }
}
