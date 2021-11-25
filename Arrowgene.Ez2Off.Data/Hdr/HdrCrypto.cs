using System;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrCrypto
    {
        private UInt32[] _0x595814 =
        {
            0x15842A91, 0x7BB3F6C8, 0xC26199A3, 0xC1EE9F2F, 0x2C16583A, 0xBFD1636E, 0x160B2C1D, 0x6DB8DAD5, 0x341A682E,
            0xB0587DE8, 0xBC5E65E2, 0xF5F4F701, 0x2F995EB6, 0x018E028F, 0xCBEB8B20, 0x55A4AAF1, 0xF9F2EF0B, 0xEDF8C715,
            0x6E37DC59, 0x8E4701C9, 0x864311C5, 0x319662A7, 0xC9EA8F23, 0xAA5549FF, 0x0E071C09, 0xA25159F3, 0x30186028,
            0x6231C453, 0x8A4509CF, 0x7C3EF842, 0xFC7EE582, 0x763BEC4D, 0x219E42BF, 0x381C7024, 0x04020806, 0x1B833698,
            0xF1F6FF07, 0x2E175C39, 0x2D985AB5, 0xE9FACF13, 0x9DC0275D, 0x02010403, 0xB7D57362, 0x924939DB, 0xA65351F5,
            0x87CD134A, 0x8FC90346, 0x51A6A2F7, 0xE271D993, 0xF7F5F302, 0x3F917EAE, 0x5DA0BAFD, 0xA1DE5F7F, 0x7038E048,
            0x1982329B, 0xD5E4B731, 0x75B4EAC1, 0x8DC80745, 0xA0505DF0, 0x1F813E9E, 0x6FB9DED6, 0xC5EC9729, 0x3C1E7822,
            0xE874CD9C, 0xCA6589AF, 0xBBD36B68, 0x11862297, 0x47AD8EEA, 0x7E3FFC41, 0xB9D26F6B, 0xD1E6BF37, 0x3E1F7C21,
            0x91C63F57, 0x99C22F5B, 0x180C3014, 0xD9E2AF3B, 0x22114433, 0xC46295A6, 0x5C2EB872, 0x10082018, 0x6A35D45F,
            0xE3FFDB1C, 0x0A05140F, 0xB25979EB, 0x0C06180A, 0xF279F98B, 0xB85C6DE4, 0x9FC1235E, 0x67BDCEDA, 0x00000000,
            0x8BCB0B40, 0xEFF9C316, 0x3B9376A8, 0xB5D47761, 0xA45255F6, 0x77B5EEC2, 0xD66BB1BD, 0x40208060, 0xE5FCD719,
            0x90483DD8, 0x73B7E6C4, 0x45AC8AE9, 0x4C26986A, 0xA7DD537A, 0x058C0A89, 0xDC6EA5B2, 0x4824906C, 0xF87CED84,
            0xFDF0E70D, 0x562BAC7D, 0x339766A4, 0xE1FEDF1F, 0x06030C05, 0x6C36D85A, 0x17852E92, 0x97C53352, 0x13872694,
            0x0F891E86, 0x89CA0F43, 0x0804100C, 0xE7FDD31A, 0xA8544DFC, 0x4E279C69, 0x038F068C, 0x49AA92E3, 0xD068BDB8,
            0x4A25946F, 0x3A1D7427, 0x9C4E25D2, 0x5E2FBC71, 0xE472D596, 0xC7ED932A, 0x4DA89AE5, 0xBE5F61E1, 0xADD84775,
            0xE673D195, 0x8C4605CA, 0xFFF1E30E, 0xC8648DAC, 0x43AF86EC, 0x6BBBD6D0, 0x6432C856, 0x65BCCAD9, 0xAFD94376,
            0x824119C3, 0xCC6685AA, 0x3219642B, 0xD7E5B332, 0xEBFBCB10, 0x361B6C2D, 0xDBE3AB38, 0xA5DC5779, 0xF078FD88,
            0xD269B9BB, 0x582CB074, 0x259C4AB9, 0x5A2DB477, 0x80401DC0, 0x5BA3B6F8, 0xFA7DE987, 0xA9DA4F73, 0x1209241B,
            0xC0609DA0, 0x35946AA1, 0xBA5D69E7, 0x1E0F3C11, 0x42218463, 0x5028A078, 0xCE6781A9, 0x743AE84E, 0xABDB4B70,
            0x41AE82EF, 0xDE6FA1B1, 0xEE77C199, 0x46238C65, 0xD3E7BB34, 0x57A5AEF2, 0xC3EF9B2C, 0x26134C35, 0x140A281E,
            0x63BFC6DC, 0x9A4D29D7, 0x2814503C, 0x44228866, 0x2B9B56B0, 0xDA6DA9B7, 0x20104030, 0x783CF044, 0x4BAB96E0,
            0x85CC1749, 0x2A15543F, 0xBDD0676D, 0x098A1283, 0xE070DD90, 0xCFE98326, 0xD46AB5BE, 0xB45A75EE, 0x1D803A9D,
            0x944A35DE, 0xAE5741F9, 0x7239E44B, 0xDFE1A33E, 0x81CE1F4F, 0xFE7FE181, 0x279D4EBA, 0x239F46BC, 0x71B6E2C7,
            0x1A0D3417, 0x6834D05C, 0xF3F7FB04, 0x95C43751, 0xB1D67F67, 0x7FB1FECE, 0x078D0E8A, 0x37956EA2, 0x59A2B2FB,
            0x79B2F2CB, 0x83CF1B4C, 0xC66391A5, 0x6633CC55, 0x9BC32B58, 0x9E4F21D1, 0xEC76C59A, 0x6030C050, 0x5229A47B,
            0x5FA1BEFE, 0xDDE0A73D, 0xEA75C99F, 0x399272AB, 0x88440DCC, 0xF67BF18D, 0x844215C6, 0x61BEC2DF, 0xB65B71ED,
            0xF47AF58E, 0xCDE88725, 0x4FA99EE6, 0x93C73B54, 0x964B31DD, 0x7A3DF447, 0x984C2DD4, 0xB3D77B64, 0xA3DF5B7C,
            0x0D881A85, 0x0B8B1680, 0xAC5645FA, 0x1C0E3812, 0xFBF3EB08, 0x69BAD2D3, 0x299A52B3, 0x7DB0FACD, 0xD86CADB4,
            0x53A7A6F4, 0x24124836, 0x3D907AAD, 0x542AA87E
        };

        private UInt32[] _0x595c14 =
        {
            0x3C281450, 0xD79A4D29, 0xC771B6E2, 0x6546238C, 0xCF8A4509, 0x0BF9F2EF, 0x9E1F813E, 0x76AFD943, 0xCB79B2F2,
            0xF155A4AA, 0xA9CE6781, 0x9115842A, 0x6DBDD067, 0x60402080, 0x19E5FCD7, 0xDA67BDCE, 0xC98E4701, 0x95E673D1,
            0xD4984C2D, 0x79A5DC57, 0xF85BA3B6, 0x4D763BEC, 0xE4B85C6D, 0x08FBF3EB, 0x9AEC76C5, 0x89058C0A, 0x121C0E38,
            0x8C038F06, 0xA0C0609D, 0xACC8648D, 0x4F81CE1F, 0xD06BBBD6, 0xC87BB3F6, 0x93E271D9, 0xE64FA99E, 0x99EE77C1,
            0x487038E0, 0x32D7E5B3, 0x90E070DD, 0x4C83CF1B, 0x5E9FC123, 0xC277B5EE, 0x8A078D0E, 0xA4339766, 0x506030C0,
            0xA7319662, 0x26CFE983, 0x03020104, 0x6F4A2594, 0x6C482490, 0xE8B0587D, 0x5B99C22F, 0x2B321964, 0xF453A7A6,
            0x64B3D77B, 0x392E175C, 0x725C2EB8, 0xDB924939, 0x74582CB0, 0x408BCB0B, 0x8BF279F9, 0xBC239F46, 0x4A87CD13,
            0x13E9FACF, 0x468FC903, 0xD29C4E25, 0xE54DA89A, 0xA237956E, 0x5195C437, 0x2FC1EE9F, 0xE04BAB96, 0xB7DA6DA9,
            0x06040208, 0x87FA7DE9, 0x3BD9E2AF, 0x61B5D477, 0x6A4C2698, 0xE349AA92, 0x04F3F7FB, 0xBA279D4E, 0x0DFDF0E7,
            0x30201040, 0x4389CA0F, 0x0C080410, 0x67B1D67F, 0xEA47AD8E, 0x70ABDB4B, 0xA3C26199, 0x0A0C0618, 0x3F2A1554,
            0xFFAA5549, 0x68BBD36B, 0x31D5E4B7, 0x775A2DB4, 0xAD3D907A, 0x8F018E02, 0x9B198232, 0xD965BCCA, 0x14180C30,
            0x596E37DC, 0x566432C8, 0x5F6A35D4, 0x81FE7FE1, 0x0F0A0514, 0xB9259C4A, 0xE7BA5D69, 0x589BC32B, 0x96E472D5,
            0x10EBFBCB, 0x785028A0, 0xB02B9B56, 0xCD7DB0FA, 0x1B120924, 0x29C5EC97, 0xC175B4EA, 0x223C1E78, 0x18100820,
            0xC5864311, 0x02F7F5F3, 0xBED46AB5, 0xE945AC8A, 0xF5A65351, 0x7CA3DF5B, 0x9D1D803A, 0x07F1F6FF, 0xBF219E42,
            0xA5C66391, 0x6EBFD163, 0x01F5F4F7, 0xF9AE5741, 0x4B7239E4, 0x417E3FFC, 0xDE944A35, 0x5493C73B, 0x73A9DA4F,
            0xB62F995E, 0x84F87CED, 0xEDB65B71, 0xBBD269B9, 0x7B5229A4, 0x5297C533, 0xA83B9376, 0xF751A6A2, 0x8EF47AF5,
            0x213E1F7C, 0x62B7D573, 0xEF41AE82, 0xC473B7E6, 0xB1DE6FA1, 0x36241248, 0x9CE874CD, 0xF257A5AE, 0x171A0D34,
            0x427C3EF8, 0xDD964B31, 0x111E0F3C, 0x20CBEB8B, 0xDC63BFC6, 0x7FA1DE5F, 0x6BB9D26F, 0xAACC6685, 0x477A3DF4,
            0x3A2C1658, 0x94138726, 0x8DF67BF1, 0x1FE1FEDF, 0x24381C70, 0x5C6834D0, 0x9FEA75C9, 0x2D361B6C, 0x5791C63F,
            0x860F891E, 0x1AE7FDD3, 0x37D1E6BF, 0xFD5DA0BA, 0x1D160B2C, 0x273A1D74, 0x981B8336, 0xB3299A52, 0x63422184,
            0xD369BAD2, 0x25CDE887, 0x16EFF9C3, 0x00000000, 0xB4D86CAD, 0xC3824119, 0x97118622, 0x75ADD847, 0xFAAC5645,
            0x5A6C36D8, 0xEBB25979, 0xF6A45255, 0xE1BE5F61, 0x9217852E, 0xC080401D, 0x66442288, 0x28301860, 0xBDD66BB1,
            0x800B8B16, 0xD19E4F21, 0x090E071C, 0x458DC807, 0x83098A12, 0xB2DC6EA5, 0xEC43AF86, 0x3526134C, 0xAB399272,
            0xC6844215, 0xF0A0505D, 0xB52D985A, 0xDF61BEC2, 0xAE3F917E, 0x0EFFF1E3, 0x44783CF0, 0x694E279C, 0x82FC7EE5,
            0x2AC7ED93, 0x7E542AA8, 0x1E140A28, 0x556633CC, 0x7AA7DD53, 0xFCA8544D, 0xCA8C4605, 0x715E2FBC, 0xA135946A,
            0xA6C46295, 0xB8D068BD, 0xEEB45A75, 0xCC88440D, 0x5D9DC027, 0x33221144, 0x0506030C, 0x2E341A68, 0xD56DB8DA,
            0xCE7FB1FE, 0x3DDDE0A7, 0x23C9EA8F, 0xFB59A2B2, 0x7D562BAC, 0x88F078FD, 0xD890483D, 0x536231C4, 0xD66FB9DE,
            0x38DBE3AB, 0x1CE3FFDB, 0x2CC3EF9B, 0xE2BC5E65, 0x34D3E7BB, 0xAFCA6589, 0x4985CC17, 0xF3A25159, 0x3EDFE1A3,
            0xFE5FA1BE, 0x4E743AE8, 0x15EDF8C7, 0x850D881A
        };

        private UInt32[] _0x595414 =
        {
            0x14503C28, 0x4D29D79A, 0xB6E2C771, 0x238C6546, 0x4509CF8A, 0xF2EF0BF9, 0x813E9E1F, 0xD94376AF, 0xB2F2CB79,
            0xA4AAF155, 0x6781A9CE, 0x842A9115, 0xD0676DBD, 0x20806040, 0xFCD719E5, 0xBDCEDA67, 0x4701C98E, 0x73D195E6,
            0x4C2DD498, 0xDC5779A5, 0xA3B6F85B, 0x3BEC4D76, 0x5C6DE4B8, 0xF3EB08FB, 0x76C59AEC, 0x8C0A8905, 0x0E38121C,
            0x8F068C03, 0x609DA0C0, 0x648DACC8, 0xCE1F4F81, 0xBBD6D06B, 0xB3F6C87B, 0x71D993E2, 0xA99EE64F, 0x77C199EE,
            0x38E04870, 0xE5B332D7, 0x70DD90E0, 0xCF1B4C83, 0xC1235E9F, 0xB5EEC277, 0x8D0E8A07, 0x9766A433, 0x30C05060,
            0x9662A731, 0xE98326CF, 0x01040302, 0x25946F4A, 0x24906C48, 0x587DE8B0, 0xC22F5B99, 0x19642B32, 0xA7A6F453,
            0xD77B64B3, 0x175C392E, 0x2EB8725C, 0x4939DB92, 0x2CB07458, 0xCB0B408B, 0x79F98BF2, 0x9F46BC23, 0xCD134A87,
            0xFACF13E9, 0xC903468F, 0x4E25D29C, 0xA89AE54D, 0x956EA237, 0xC4375195, 0xEE9F2FC1, 0xAB96E04B, 0x6DA9B7DA,
            0x02080604, 0x7DE987FA, 0xE2AF3BD9, 0xD47761B5, 0x26986A4C, 0xAA92E349, 0xF7FB04F3, 0x9D4EBA27, 0xF0E70DFD,
            0x10403020, 0xCA0F4389, 0x04100C08, 0xD67F67B1, 0xAD8EEA47, 0xDB4B70AB, 0x6199A3C2, 0x06180A0C, 0x15543F2A,
            0x5549FFAA, 0xD36B68BB, 0xE4B731D5, 0x2DB4775A, 0x907AAD3D, 0x8E028F01, 0x82329B19, 0xBCCAD965, 0x0C301418,
            0x37DC596E, 0x32C85664, 0x35D45F6A, 0x7FE181FE, 0x05140F0A, 0x9C4AB925, 0x5D69E7BA, 0xC32B589B, 0x72D596E4,
            0xFBCB10EB, 0x28A07850, 0x9B56B02B, 0xB0FACD7D, 0x09241B12, 0xEC9729C5, 0xB4EAC175, 0x1E78223C, 0x08201810,
            0x4311C586, 0xF5F302F7, 0x6AB5BED4, 0xAC8AE945, 0x5351F5A6, 0xDF5B7CA3, 0x803A9D1D, 0xF6FF07F1, 0x9E42BF21,
            0x6391A5C6, 0xD1636EBF, 0xF4F701F5, 0x5741F9AE, 0x39E44B72, 0x3FFC417E, 0x4A35DE94, 0xC73B5493, 0xDA4F73A9,
            0x995EB62F, 0x7CED84F8, 0x5B71EDB6, 0x69B9BBD2, 0x29A47B52, 0xC5335297, 0x9376A83B, 0xA6A2F751, 0x7AF58EF4,
            0x1F7C213E, 0xD57362B7, 0xAE82EF41, 0xB7E6C473, 0x6FA1B1DE, 0x12483624, 0x74CD9CE8, 0xA5AEF257, 0x0D34171A,
            0x3EF8427C, 0x4B31DD96, 0x0F3C111E, 0xEB8B20CB, 0xBFC6DC63, 0xDE5F7FA1, 0xD26F6BB9, 0x6685AACC, 0x3DF4477A,
            0x16583A2C, 0x87269413, 0x7BF18DF6, 0xFEDF1FE1, 0x1C702438, 0x34D05C68, 0x75C99FEA, 0x1B6C2D36, 0xC63F5791,
            0x891E860F, 0xFDD31AE7, 0xE6BF37D1, 0xA0BAFD5D, 0x0B2C1D16, 0x1D74273A, 0x8336981B, 0x9A52B329, 0x21846342,
            0xBAD2D369, 0xE88725CD, 0xF9C316EF, 0x00000000, 0x6CADB4D8, 0x4119C382, 0x86229711, 0xD84775AD, 0x5645FAAC,
            0x36D85A6C, 0x5979EBB2, 0x5255F6A4, 0x5F61E1BE, 0x852E9217, 0x401DC080, 0x22886644, 0x18602830, 0x6BB1BDD6,
            0x8B16800B, 0x4F21D19E, 0x071C090E, 0xC807458D, 0x8A128309, 0x6EA5B2DC, 0xAF86EC43, 0x134C3526, 0x9272AB39,
            0x4215C684, 0x505DF0A0, 0x985AB52D, 0xBEC2DF61, 0x917EAE3F, 0xF1E30EFF, 0x3CF04478, 0x279C694E, 0x7EE582FC,
            0xED932AC7, 0x2AA87E54, 0x0A281E14, 0x33CC5566, 0xDD537AA7, 0x544DFCA8, 0x4605CA8C, 0x2FBC715E, 0x946AA135,
            0x6295A6C4, 0x68BDB8D0, 0x5A75EEB4, 0x440DCC88, 0xC0275D9D, 0x11443322, 0x030C0506, 0x1A682E34, 0xB8DAD56D,
            0xB1FECE7F, 0xE0A73DDD, 0xEA8F23C9, 0xA2B2FB59, 0x2BAC7D56, 0x78FD88F0, 0x483DD890, 0x31C45362, 0xB9DED66F,
            0xE3AB38DB, 0xFFDB1CE3, 0xEF9B2CC3, 0x5E65E2BC, 0xE7BB34D3, 0x6589AFCA, 0xCC174985, 0x5159F3A2, 0xE1A33EDF,
            0xA1BEFE5F, 0x3AE84E74, 0xF8C715ED, 0x881A850D
        };

        private UInt32[] _0x596014 =
        {
            0x2A911584, 0xF6C87BB3, 0x99A3C261, 0x9F2FC1EE, 0x583A2C16, 0x636EBFD1, 0x2C1D160B, 0xDAD56DB8, 0x682E341A,
            0x7DE8B058, 0x65E2BC5E, 0xF701F5F4, 0x5EB62F99, 0x028F018E, 0x8B20CBEB, 0xAAF155A4, 0xEF0BF9F2, 0xC715EDF8,
            0xDC596E37, 0x01C98E47, 0x11C58643, 0x62A73196, 0x8F23C9EA, 0x49FFAA55, 0x1C090E07, 0x59F3A251, 0x60283018,
            0xC4536231, 0x09CF8A45, 0xF8427C3E, 0xE582FC7E, 0xEC4D763B, 0x42BF219E, 0x7024381C, 0x08060402, 0x36981B83,
            0xFF07F1F6, 0x5C392E17, 0x5AB52D98, 0xCF13E9FA, 0x275D9DC0, 0x04030201, 0x7362B7D5, 0x39DB9249, 0x51F5A653,
            0x134A87CD, 0x03468FC9, 0xA2F751A6, 0xD993E271, 0xF302F7F5, 0x7EAE3F91, 0xBAFD5DA0, 0x5F7FA1DE, 0xE0487038,
            0x329B1982, 0xB731D5E4, 0xEAC175B4, 0x07458DC8, 0x5DF0A050, 0x3E9E1F81, 0xDED66FB9, 0x9729C5EC, 0x78223C1E,
            0xCD9CE874, 0x89AFCA65, 0x6B68BBD3, 0x22971186, 0x8EEA47AD, 0xFC417E3F, 0x6F6BB9D2, 0xBF37D1E6, 0x7C213E1F,
            0x3F5791C6, 0x2F5B99C2, 0x3014180C, 0xAF3BD9E2, 0x44332211, 0x95A6C462, 0xB8725C2E, 0x20181008, 0xD45F6A35,
            0xDB1CE3FF, 0x140F0A05, 0x79EBB259, 0x180A0C06, 0xF98BF279, 0x6DE4B85C, 0x235E9FC1, 0xCEDA67BD, 0x00000000,
            0x0B408BCB, 0xC316EFF9, 0x76A83B93, 0x7761B5D4, 0x55F6A452, 0xEEC277B5, 0xB1BDD66B, 0x80604020, 0xD719E5FC,
            0x3DD89048, 0xE6C473B7, 0x8AE945AC, 0x986A4C26, 0x537AA7DD, 0x0A89058C, 0xA5B2DC6E, 0x906C4824, 0xED84F87C,
            0xE70DFDF0, 0xAC7D562B, 0x66A43397, 0xDF1FE1FE, 0x0C050603, 0xD85A6C36, 0x2E921785, 0x335297C5, 0x26941387,
            0x1E860F89, 0x0F4389CA, 0x100C0804, 0xD31AE7FD, 0x4DFCA854, 0x9C694E27, 0x068C038F, 0x92E349AA, 0xBDB8D068,
            0x946F4A25, 0x74273A1D, 0x25D29C4E, 0xBC715E2F, 0xD596E472, 0x932AC7ED, 0x9AE54DA8, 0x61E1BE5F, 0x4775ADD8,
            0xD195E673, 0x05CA8C46, 0xE30EFFF1, 0x8DACC864, 0x86EC43AF, 0xD6D06BBB, 0xC8566432, 0xCAD965BC, 0x4376AFD9,
            0x19C38241, 0x85AACC66, 0x642B3219, 0xB332D7E5, 0xCB10EBFB, 0x6C2D361B, 0xAB38DBE3, 0x5779A5DC, 0xFD88F078,
            0xB9BBD269, 0xB074582C, 0x4AB9259C, 0xB4775A2D, 0x1DC08040, 0xB6F85BA3, 0xE987FA7D, 0x4F73A9DA, 0x241B1209,
            0x9DA0C060, 0x6AA13594, 0x69E7BA5D, 0x3C111E0F, 0x84634221, 0xA0785028, 0x81A9CE67, 0xE84E743A, 0x4B70ABDB,
            0x82EF41AE, 0xA1B1DE6F, 0xC199EE77, 0x8C654623, 0xBB34D3E7, 0xAEF257A5, 0x9B2CC3EF, 0x4C352613, 0x281E140A,
            0xC6DC63BF, 0x29D79A4D, 0x503C2814, 0x88664422, 0x56B02B9B, 0xA9B7DA6D, 0x40302010, 0xF044783C, 0x96E04BAB,
            0x174985CC, 0x543F2A15, 0x676DBDD0, 0x1283098A, 0xDD90E070, 0x8326CFE9, 0xB5BED46A, 0x75EEB45A, 0x3A9D1D80,
            0x35DE944A, 0x41F9AE57, 0xE44B7239, 0xA33EDFE1, 0x1F4F81CE, 0xE181FE7F, 0x4EBA279D, 0x46BC239F, 0xE2C771B6,
            0x34171A0D, 0xD05C6834, 0xFB04F3F7, 0x375195C4, 0x7F67B1D6, 0xFECE7FB1, 0x0E8A078D, 0x6EA23795, 0xB2FB59A2,
            0xF2CB79B2, 0x1B4C83CF, 0x91A5C663, 0xCC556633, 0x2B589BC3, 0x21D19E4F, 0xC59AEC76, 0xC0506030, 0xA47B5229,
            0xBEFE5FA1, 0xA73DDDE0, 0xC99FEA75, 0x72AB3992, 0x0DCC8844, 0xF18DF67B, 0x15C68442, 0xC2DF61BE, 0x71EDB65B,
            0xF58EF47A, 0x8725CDE8, 0x9EE64FA9, 0x3B5493C7, 0x31DD964B, 0xF4477A3D, 0x2DD4984C, 0x7B64B3D7, 0x5B7CA3DF,
            0x1A850D88, 0x16800B8B, 0x45FAAC56, 0x38121C0E, 0xEB08FBF3, 0xD2D369BA, 0x52B3299A, 0xFACD7DB0, 0xADB4D86C,
            0xA6F453A7, 0x48362412, 0x7AAD3D90, 0xA87E542A
        };

        private UInt32[] _0x594C14 =
        {
            0x00000014, 0x0000004D, 0x000000B6, 0x00000023, 0x00000045, 0x000000F2, 0x00000081, 0x000000D9, 0x000000B2,
            0x000000A4, 0x00000067, 0x00000084, 0x000000D0, 0x00000020, 0x000000FC, 0x000000BD, 0x00000047, 0x00000073,
            0x0000004C, 0x000000DC, 0x000000A3, 0x0000003B, 0x0000005C, 0x000000F3, 0x00000076, 0x0000008C, 0x0000000E,
            0x0000008F, 0x00000060, 0x00000064, 0x000000CE, 0x000000BB, 0x000000B3, 0x00000071, 0x000000A9, 0x00000077,
            0x00000038, 0x000000E5, 0x00000070, 0x000000CF, 0x000000C1, 0x000000B5, 0x0000008D, 0x00000097, 0x00000030,
            0x00000096, 0x000000E9, 0x00000001, 0x00000025, 0x00000024, 0x00000058, 0x000000C2, 0x00000019, 0x000000A7,
            0x000000D7, 0x00000017, 0x0000002E, 0x00000049, 0x0000002C, 0x000000CB, 0x00000079, 0x0000009F, 0x000000CD,
            0x000000FA, 0x000000C9, 0x0000004E, 0x000000A8, 0x00000095, 0x000000C4, 0x000000EE, 0x000000AB, 0x0000006D,
            0x00000002, 0x0000007D, 0x000000E2, 0x000000D4, 0x00000026, 0x000000AA, 0x000000F7, 0x0000009D, 0x000000F0,
            0x00000010, 0x000000CA, 0x00000004, 0x000000D6, 0x000000AD, 0x000000DB, 0x00000061, 0x00000006, 0x00000015,
            0x00000055, 0x000000D3, 0x000000E4, 0x0000002D, 0x00000090, 0x0000008E, 0x00000082, 0x000000BC, 0x0000000C,
            0x00000037, 0x00000032, 0x00000035, 0x0000007F, 0x00000005, 0x0000009C, 0x0000005D, 0x000000C3, 0x00000072,
            0x000000FB, 0x00000028, 0x0000009B, 0x000000B0, 0x00000009, 0x000000EC, 0x000000B4, 0x0000001E, 0x00000008,
            0x00000043, 0x000000F5, 0x0000006A, 0x000000AC, 0x00000053, 0x000000DF, 0x00000080, 0x000000F6, 0x0000009E,
            0x00000063, 0x000000D1, 0x000000F4, 0x00000057, 0x00000039, 0x0000003F, 0x0000004A, 0x000000C7, 0x000000DA,
            0x00000099, 0x0000007C, 0x0000005B, 0x00000069, 0x00000029, 0x000000C5, 0x00000093, 0x000000A6, 0x0000007A,
            0x0000001F, 0x000000D5, 0x000000AE, 0x000000B7, 0x0000006F, 0x00000012, 0x00000074, 0x000000A5, 0x0000000D,
            0x0000003E, 0x0000004B, 0x0000000F, 0x000000EB, 0x000000BF, 0x000000DE, 0x000000D2, 0x00000066, 0x0000003D,
            0x00000016, 0x00000087, 0x0000007B, 0x000000FE, 0x0000001C, 0x00000034, 0x00000075, 0x0000001B, 0x000000C6,
            0x00000089, 0x000000FD, 0x000000E6, 0x000000A0, 0x0000000B, 0x0000001D, 0x00000083, 0x0000009A, 0x00000021,
            0x000000BA, 0x000000E8, 0x000000F9, 0x00000000, 0x0000006C, 0x00000041, 0x00000086, 0x000000D8, 0x00000056,
            0x00000036, 0x00000059, 0x00000052, 0x0000005F, 0x00000085, 0x00000040, 0x00000022, 0x00000018, 0x0000006B,
            0x0000008B, 0x0000004F, 0x00000007, 0x000000C8, 0x0000008A, 0x0000006E, 0x000000AF, 0x00000013, 0x00000092,
            0x00000042, 0x00000050, 0x00000098, 0x000000BE, 0x00000091, 0x000000F1, 0x0000003C, 0x00000027, 0x0000007E,
            0x000000ED, 0x0000002A, 0x0000000A, 0x00000033, 0x000000DD, 0x00000054, 0x00000046, 0x0000002F, 0x00000094,
            0x00000062, 0x00000068, 0x0000005A, 0x00000044, 0x000000C0, 0x00000011, 0x00000003, 0x0000001A, 0x000000B8,
            0x000000B1, 0x000000E0, 0x000000EA, 0x000000A2, 0x0000002B, 0x00000078, 0x00000048, 0x00000031, 0x000000B9,
            0x000000E3, 0x000000FF, 0x000000EF, 0x0000005E, 0x000000E7, 0x00000065, 0x000000CC, 0x00000051, 0x000000E1,
            0x000000A1, 0x0000003A, 0x000000F8, 0x00000088
        };

        private UInt32[] _0x595014 =
        {
            0x00000084, 0x000000B3, 0x00000061, 0x000000EE, 0x00000016, 0x000000D1, 0x0000000B, 0x000000B8, 0x0000001A,
            0x00000058, 0x0000005E, 0x000000F4, 0x00000099, 0x0000008E, 0x000000EB, 0x000000A4, 0x000000F2, 0x000000F8,
            0x00000037, 0x00000047, 0x00000043, 0x00000096, 0x000000EA, 0x00000055, 0x00000007, 0x00000051, 0x00000018,
            0x00000031, 0x00000045, 0x0000003E, 0x0000007E, 0x0000003B, 0x0000009E, 0x0000001C, 0x00000002, 0x00000083,
            0x000000F6, 0x00000017, 0x00000098, 0x000000FA, 0x000000C0, 0x00000001, 0x000000D5, 0x00000049, 0x00000053,
            0x000000CD, 0x000000C9, 0x000000A6, 0x00000071, 0x000000F5, 0x00000091, 0x000000A0, 0x000000DE, 0x00000038,
            0x00000082, 0x000000E4, 0x000000B4, 0x000000C8, 0x00000050, 0x00000081, 0x000000B9, 0x000000EC, 0x0000001E,
            0x00000074, 0x00000065, 0x000000D3, 0x00000086, 0x000000AD, 0x0000003F, 0x000000D2, 0x000000E6, 0x0000001F,
            0x000000C6, 0x000000C2, 0x0000000C, 0x000000E2, 0x00000011, 0x00000062, 0x0000002E, 0x00000008, 0x00000035,
            0x000000FF, 0x00000005, 0x00000059, 0x00000006, 0x00000079, 0x0000005C, 0x000000C1, 0x000000BD, 0x00000000,
            0x000000CB, 0x000000F9, 0x00000093, 0x000000D4, 0x00000052, 0x000000B5, 0x0000006B, 0x00000020, 0x000000FC,
            0x00000048, 0x000000B7, 0x000000AC, 0x00000026, 0x000000DD, 0x0000008C, 0x0000006E, 0x00000024, 0x0000007C,
            0x000000F0, 0x0000002B, 0x00000097, 0x000000FE, 0x00000003, 0x00000036, 0x00000085, 0x000000C5, 0x00000087,
            0x00000089, 0x000000CA, 0x00000004, 0x000000FD, 0x00000054, 0x00000027, 0x0000008F, 0x000000AA, 0x00000068,
            0x00000025, 0x0000001D, 0x0000004E, 0x0000002F, 0x00000072, 0x000000ED, 0x000000A8, 0x0000005F, 0x000000D8,
            0x00000073, 0x00000046, 0x000000F1, 0x00000064, 0x000000AF, 0x000000BB, 0x00000032, 0x000000BC, 0x000000D9,
            0x00000041, 0x00000066, 0x00000019, 0x000000E5, 0x000000FB, 0x0000001B, 0x000000E3, 0x000000DC, 0x00000078,
            0x00000069, 0x0000002C, 0x0000009C, 0x0000002D, 0x00000040, 0x000000A3, 0x0000007D, 0x000000DA, 0x00000009,
            0x00000060, 0x00000094, 0x0000005D, 0x0000000F, 0x00000021, 0x00000028, 0x00000067, 0x0000003A, 0x000000DB,
            0x000000AE, 0x0000006F, 0x00000077, 0x00000023, 0x000000E7, 0x000000A5, 0x000000EF, 0x00000013, 0x0000000A,
            0x000000BF, 0x0000004D, 0x00000014, 0x00000022, 0x0000009B, 0x0000006D, 0x00000010, 0x0000003C, 0x000000AB,
            0x000000CC, 0x00000015, 0x000000D0, 0x0000008A, 0x00000070, 0x000000E9, 0x0000006A, 0x0000005A, 0x00000080,
            0x0000004A, 0x00000057, 0x00000039, 0x000000E1, 0x000000CE, 0x0000007F, 0x0000009D, 0x0000009F, 0x000000B6,
            0x0000000D, 0x00000034, 0x000000F7, 0x000000C4, 0x000000D6, 0x000000B1, 0x0000008D, 0x00000095, 0x000000A2,
            0x000000B2, 0x000000CF, 0x00000063, 0x00000033, 0x000000C3, 0x0000004F, 0x00000076, 0x00000030, 0x00000029,
            0x000000A1, 0x000000E0, 0x00000075, 0x00000092, 0x00000044, 0x0000007B, 0x00000042, 0x000000BE, 0x0000005B,
            0x0000007A, 0x000000E8, 0x000000A9, 0x000000C7, 0x0000004B, 0x0000003D, 0x0000004C, 0x000000D7, 0x000000DF,
            0x00000088, 0x0000008B, 0x00000056, 0x0000000E, 0x000000F3, 0x000000BA, 0x0000009A, 0x000000B0, 0x0000006C,
            0x000000A7, 0x00000012, 0x00000090, 0x0000002A
        };

        public static HdrCrypto Instance = new HdrCrypto();

        public HdrCrypto()
        {
        }

        /// <summary>
        /// Checks if the source is encrypted.
        /// </summary>
        /// <returns>true or false, null if it can not be determined</returns>
        public bool? IsEncrypted(byte[] source, KeyState keyState)
        {
            if (keyState.Header.Count == 0)
            {
                return null;
            }

            foreach (byte[] header in keyState.Header)
            {
                bool equal = true;
                for (int i = 0; i < header.Length; i++)
                {
                    if (header[i] != source[i])
                    {
                        equal = false;
                        break;
                    }
                }

                if (equal)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] Decrypt(byte[] source, KeyState keyState)
        {
            UInt32 offset = 0;
            UInt32 fsize = (UInt32) source.Length;
            UInt32 currentptr = 0;
            UInt32 remainbyte = 0;
            UInt32 fsize_ = 0;
            UInt32 fsize__ = 0;
            UInt32 remainbyte2 = 0;
            byte[] result = new byte[fsize];
            do
            {
                KeyState key1 = (KeyState) keyState.Clone();
                if (fsize >= 0x1000)
                {
                    fsize -= 0x1000;
                    currentptr += 0x1000;
                    fsize_ = 0x1000 >> 4;
                }
                else
                {
                    remainbyte = fsize & 0x8000000F;
                    fsize_ = fsize >> 4;
                    fsize = 0;
                }

                if (remainbyte == 0)
                {
                    do
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                        }

                        Permutation(key1);
                        for (int i = 0; i < 4; i++)
                        {
                            UInt32 _out = Out(key1, i);
                            Assign(key1, i);
                            int sub_shr = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                int shr = 0x18;
                                shr -= sub_shr;
                                result[j + offset] = (byte) (_out >> shr);
                                sub_shr += 0x8;
                            }

                            offset += 4;
                        }

                        fsize_--;
                    } while (fsize_ != 0);

                    continue;
                }

                if (fsize_ > 0)
                {
                    UInt32 jj = fsize_;
                    do
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                        }

                        Permutation(key1);
                        for (int i = 0; i < 4; i++)
                        {
                            UInt32 _out = Out(key1, i);
                            Assign(key1, i);
                            int sub_shr = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                int shr = 0x18;
                                shr -= sub_shr;
                                result[j + offset] = (byte) (_out >> shr);
                                sub_shr += 0x8;
                            }

                            offset += 4;
                        }

                        jj--;
                    } while (jj != 0);
                }

                remainbyte2 = remainbyte & 0x80000003;
                fsize__ = remainbyte >> 2;
                if (key1.CryptoExtension == HdrCryptoExtension.ptn ||
                    key1.CryptoExtension == HdrCryptoExtension.dat ||
                    key1.CryptoExtension == HdrCryptoExtension.str)
                {
                    while (offset < source.Length)
                    {
                        result[offset] = source[offset];
                        offset++;
                    }
                }
                else if (remainbyte2 == 0)
                {
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            do
                            {
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                                j++;
                            } while (j < 4);

                            j = 0;
                            currentptr2 += 4;
                            i++;
                        } while (i < fsize__);

                        currentptr2 = currentptr + (fsize_ << 4);
                        Permutation(key1);
                        i = 0;
                        j = 0;
                        do
                        {
                            int shr_ = 0;
                            UInt32 _out = Out(key1, i);
                            do
                            {
                                int shr = 0x18;
                                shr -= shr_;
                                shr_ += 8;
                                result[j + offset] = (byte) (_out >> shr);
                                j++;
                            } while (j < 4);

                            offset += 4;
                            i++;
                            j = 0;
                            currentptr2 += 4;
                        } while (i < fsize__);
                    }
                }
                else
                {
                    UInt32 count = 0;
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            do
                            {
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                                j++;
                                count++;
                            } while (j < 4);

                            j = 0;
                            currentptr2 += 4;
                            i++;
                        } while (i < fsize__);
                    }

                    if (remainbyte2 > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4) + count;

                        for (int i = 0; i < remainbyte2; i++)
                        {
                            key1.Key[fsize__] = (key1.Key[fsize__] << 8) ^ source[i + offset + count];
                        }
                    }

                    key1.Key[fsize__] = key1.Key[fsize__] << (int) (0x20 - (remainbyte2 * 8));
                    Permutation(key1);
                    UInt32 _out;
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            int shr_ = 0;
                            _out = Out(key1, i);
                            do
                            {
                                int shr = 0x18;
                                shr -= shr_;
                                shr_ += 8;
                                result[j + offset] = (byte) (_out >> shr);
                                currentptr++;
                                j++;
                            } while (j < 4);

                            offset += 4;
                            i++;
                            j = 0;
                            currentptr2 += 4;
                        } while (i < fsize__);
                    }

                    _out = Out(key1, (int) fsize__);
                    if (remainbyte2 > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        byte esi = 0;
                        do
                        {
                            byte ecx = 0x18;
                            ecx -= esi;
                            esi += 8;
                            result[offset] = (byte) (_out >> ecx);
                            offset++;
                            i++;
                        } while (i < remainbyte2);
                    }
                }
            } while (fsize != 0);

            return result;
        }


        public byte[] Encrypt(byte[] source, KeyState keyState)
        {
            UInt32 offset = 0;
            UInt32 fsize = (UInt32) source.Length;
            UInt32 currentptr = 0;
            UInt32 remainbyte = 0;
            UInt32 fsize_ = 0;
            UInt32 fsize__ = 0;
            UInt32 remainbyte2 = 0;
            byte[] result = new byte[fsize];
            do
            {
                KeyState key1 = (KeyState) keyState.Clone();
                if (fsize >= 0x1000)
                {
                    fsize -= 0x1000;
                    currentptr += 0x1000;
                    fsize_ = 0x1000 >> 4;
                }
                else
                {
                    remainbyte = fsize & 0x8000000F;
                    fsize_ = fsize >> 4;
                    fsize = 0;
                }

                if (remainbyte == 0)
                {
                    do
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                        }

                        Permutation(key1);
                        for (int i = 0; i < 4; i++)
                        {
                            UInt32 _out = OutE(key1, i);
                            Assign(key1, i);
                         //   keyState.Init[i] = _out;

                            int sub_shr = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                int shr = 0x18;
                                shr -= sub_shr;
                                result[j + offset] = (byte) (_out >> shr);
                                sub_shr += 0x8;
                            }

                            offset += 4;
                        }

                        fsize_--;
                    } while (fsize_ != 0);

                    continue;
                }

                if (fsize_ > 0)
                {
                    UInt32 jj = fsize_;
                    do
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                        }

                        Permutation(key1);
                        for (int i = 0; i < 4; i++)
                        {
                            UInt32 _out = OutE(key1, i);
                            AssignE(key1, i, _out);
                            int sub_shr = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                int shr = 0x18;
                                shr -= sub_shr;
                                result[j + offset] = (byte) (_out >> shr);
                                sub_shr += 0x8;
                            }

                            offset += 4;
                        }

                        jj--;
                    } while (jj != 0);
                }

                remainbyte2 = remainbyte & 0x80000003;
                fsize__ = remainbyte >> 2;
                if (key1.CryptoExtension == HdrCryptoExtension.ptn ||
                    key1.CryptoExtension == HdrCryptoExtension.dat ||
                    key1.CryptoExtension == HdrCryptoExtension.str)
                {
                    while (offset < source.Length)
                    {
                        result[offset] = source[offset];
                        offset++;
                    }
                }
                else if (remainbyte2 == 0)
                {
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            do
                            {
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                                j++;
                            } while (j < 4);

                            j = 0;
                            currentptr2 += 4;
                            i++;
                        } while (i < fsize__);

                        currentptr2 = currentptr + (fsize_ << 4);
                        Permutation(key1);
                        i = 0;
                        j = 0;
                        do
                        {
                            int shr_ = 0;
                            UInt32 _out = OutE(key1, i);
                            do
                            {
                                int shr = 0x18;
                                shr -= shr_;
                                shr_ += 8;
                                result[j + offset] = (byte) (_out >> shr);
                                j++;
                            } while (j < 4);

                            offset += 4;
                            i++;
                            j = 0;
                            currentptr2 += 4;
                        } while (i < fsize__);
                    }
                }
                else
                {
                    UInt32 count = 0;
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            do
                            {
                                key1.Key[i] = (key1.Key[i] << 8) ^ source[(i * 4) + j + offset];
                                j++;
                                count++;
                            } while (j < 4);

                            j = 0;
                            currentptr2 += 4;
                            i++;
                        } while (i < fsize__);
                    }

                    if (remainbyte2 > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4) + count;

                        for (int i = 0; i < remainbyte2; i++)
                        {
                            key1.Key[fsize__] = (key1.Key[fsize__] << 8) ^ source[i + offset + count];
                        }
                    }

                    key1.Key[fsize__] = key1.Key[fsize__] << (int) (0x20 - (remainbyte2 * 8));
                    Permutation(key1);
                    UInt32 _out;
                    if (fsize__ > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        int j = 0;
                        do
                        {
                            int shr_ = 0;
                            _out = OutE(key1, i);
                            do
                            {
                                int shr = 0x18;
                                shr -= shr_;
                                shr_ += 8;
                                result[j + offset] = (byte) (_out >> shr);
                                currentptr++;
                                j++;
                            } while (j < 4);

                            offset += 4;
                            i++;
                            j = 0;
                            currentptr2 += 4;
                        } while (i < fsize__);
                    }

                    _out = OutE(key1, (int) fsize__);
                    if (remainbyte2 > 0)
                    {
                        UInt32 currentptr2 = currentptr + (fsize_ << 4);
                        int i = 0;
                        byte esi = 0;
                        do
                        {
                            byte ecx = 0x18;
                            ecx -= esi;
                            esi += 8;
                            result[offset] = (byte) (_out >> ecx);
                            offset++;
                            i++;
                        } while (i < remainbyte2);
                    }
                }
            } while (fsize != 0);

            return result;
        }

        private void Permutation(KeyState keyState)
        {
            switch (keyState.CryptoExtension)
            {
                case HdrCryptoExtension.png:
                case HdrCryptoExtension.ogg:
                case HdrCryptoExtension.jpg:
                case HdrCryptoExtension.bin:
                case HdrCryptoExtension.pvi:
                case HdrCryptoExtension.scr:
                case HdrCryptoExtension.bmp:
                    fun_4d7030(keyState);
                    break;
                case HdrCryptoExtension.str:
                case HdrCryptoExtension.dat:
                case HdrCryptoExtension.ptn:
                    fun_4D8160(keyState);
                    break;
            }
        }

        private UInt32 Out(KeyState keyState, int index)
        {
            switch (keyState.CryptoExtension)
            {
                case HdrCryptoExtension.png:
                case HdrCryptoExtension.ogg:
                case HdrCryptoExtension.jpg:
                case HdrCryptoExtension.bin:
                case HdrCryptoExtension.pvi:
                case HdrCryptoExtension.scr:
                case HdrCryptoExtension.bmp:
                    return keyState.Output[index] ^ keyState.Key[index];
                case HdrCryptoExtension.str:
                case HdrCryptoExtension.dat:
                case HdrCryptoExtension.ptn:
                    return keyState.Output[index] ^ keyState.Init[index];
            }

            return 0;
        }


        private UInt32 OutE(KeyState keyState, int index)
        {
            switch (keyState.CryptoExtension)
            {
                case HdrCryptoExtension.png:
                case HdrCryptoExtension.ogg:
                case HdrCryptoExtension.jpg:
                case HdrCryptoExtension.bin:
                case HdrCryptoExtension.pvi:
                case HdrCryptoExtension.scr:
                case HdrCryptoExtension.bmp:
                    return keyState.Output[index] ^ keyState.Key[index];
                case HdrCryptoExtension.str:
                case HdrCryptoExtension.dat:
                case HdrCryptoExtension.ptn:
                    return keyState.Output[index] ^ keyState.Init[index];
            }

            return 0;
        }

        private void Assign(KeyState keyState, int index)
        {
            switch (keyState.CryptoExtension)
            {
                case HdrCryptoExtension.png:
                case HdrCryptoExtension.ogg:
                case HdrCryptoExtension.jpg:
                case HdrCryptoExtension.str:
                case HdrCryptoExtension.dat:
                case HdrCryptoExtension.ptn:
                case HdrCryptoExtension.bmp:
                    keyState.Init[index] = keyState.Key[index];
                    break;
                case HdrCryptoExtension.bin:
                case HdrCryptoExtension.pvi:
                case HdrCryptoExtension.scr:
                    keyState.Init[index] = keyState.Output[index];
                    break;
            }
        }

        private void AssignE(KeyState keyState, int index, UInt32 _out)
        {
            switch (keyState.CryptoExtension)
            {
                case HdrCryptoExtension.png:
                case HdrCryptoExtension.ogg:
                case HdrCryptoExtension.jpg:
                case HdrCryptoExtension.str:
                case HdrCryptoExtension.dat:
                case HdrCryptoExtension.ptn:
                case HdrCryptoExtension.bmp:
                    keyState.Init[index] = _out;
                    break;
                case HdrCryptoExtension.bin:
                case HdrCryptoExtension.pvi:
                case HdrCryptoExtension.scr:
                    keyState.Init[index] = keyState.Output[index];
                    break;
            }
        }


        private void fun_4D8160(KeyState keyState)
        {
            uint a = keyState.Hash[68] + keyState.Key[0];
            uint b = keyState.Hash[71] ^ keyState.Key[3];
            byte c = (byte) (a >> 8);
            byte d = (byte) (a >> 16);
            uint e = _0x595c14[c];
            uint f = a & 0x000000ff;
            uint g = _0x595814[d];
            uint h = a >> 0x18;
            uint i = g ^ e;
            uint j = _0x595414[h];
            uint k = _0x596014[f];
            uint l = i ^ j;
            uint m = l ^ k;
            byte n = (byte) (m >> 16);
            uint o = m >> 0x18;
            uint p = _0x595014[n];
            uint q = _0x594C14[o];
            uint r = q << 0x8;
            uint s = p ^ r;
            byte t = (byte) (m >> 8);
            uint u = m & 0x000000ff;
            uint v = s << 0x8;
            uint w = _0x594C14[t];
            uint x = _0x595014[u];
            uint y = v ^ w;
            uint z = y << 0x8;
            uint aa = z ^ x;
            uint ab = keyState.Key[1] ^ keyState.Hash[69];
            uint ac = aa + ab;
            uint ad = keyState.Key[2] + keyState.Hash[70];
            uint ae = ad ^ aa;
            byte af = (byte) (ac >> 16);
            byte ag = (byte) (ac >> 8);
            uint ah = _0x595814[af];
            uint ai = _0x595c14[ag];
            uint aj = ah ^ ai;
            uint ak = ac >> 0x18;
            uint al = _0x595414[ak];
            uint am = ac & 0x000000ff;
            uint an = aj ^ al;
            uint ao = _0x596014[am];
            uint ap = an ^ ao;
            byte aq = (byte) (ap >> 16);
            uint ar = ap >> 0x18;
            uint at = _0x594C14[ar];
            uint au = at << 0x8;
            uint av = _0x595014[aq];
            uint aw = av ^ au;
            byte ax = (byte) (ap >> 8);
            uint ay = ap & 0x000000ff;
            uint az = aw << 0x8;
            uint ba = az ^ _0x594C14[ax];
            uint bb = _0x595014[ay];
            uint bc = ba << 0x8;
            uint bd = bc ^ bb;
            uint be = a & 0x0000001f;
            uint bf = ae + bd;
            uint bg = 0x20 - be;
            uint bh = b >> (byte) bg;
            uint bi = ac & 0x0000001f;
            uint bj = b << (byte) be;
            uint bk = bh | bj;
            byte bl = (byte) (bf >> 8);
            uint bm = bk ^ bd;
            uint bn = _0x595c14[bl];
            uint bo = (byte) (bf >> 16);
            uint bp = _0x595814[bo];
            uint bq = bp ^ bn;
            uint br = bf >> 0x18;
            uint bs = _0x595414[br];
            uint bt = bf & 0x000000ff;
            uint bu = bq ^ bs;
            uint bv = _0x596014[bt];
            uint bw = bu ^ bv;
            uint bx = (byte) (bw >> 16);
            uint by = bw >> 0x18;
            uint bz = _0x594C14[by];
            uint ca = _0x595014[bx];
            uint cb = bz << 0x8;
            uint cc = (byte) (bw >> 8);
            uint cd = bw & 0x000000ff;
            uint ce = ca ^ cb;
            uint cf = _0x594C14[cc];
            uint cg = _0x595014[cd];
            uint ch = ce << 0x8;
            uint ci = 0x20 - bi;
            uint cj = ch ^ cf;
            uint ck = a >> (byte) ci;
            uint cl = cj << 0x8;
            uint cm = a << (byte) bi;
            uint cn = cl ^ cg;
            uint co = bm + cn;
            uint cp = ck | cm;
            uint cq = cp ^ cn;
            byte cr = (byte) (co >> 16);
            uint cs = _0x595814[cr];
            byte ct = (byte) (co >> 8);
            uint cu = cs ^ _0x595c14[ct];
            uint cv = co & 0x000000ff;
            uint cw = co >> 0x18;
            uint cx = _0x596014[cv];
            uint cy = _0x595414[cw];
            uint cz = cu ^ cy;
            uint da = cz ^ cx;
            byte db = (byte) (da >> 16);
            uint dc = da >> 0x18;
            uint dd = _0x594C14[dc];
            uint de = _0x595014[db];
            uint df = dd << 0x8;
            byte dg = (byte) (da >> 8);
            uint dh = de ^ df;
            uint di = dh << 0x8;
            uint dj = _0x594C14[dg];
            uint dk = da & 0x000000ff;
            uint dl = di ^ dj;
            uint dm = _0x595014[dk];
            uint dn = bf & 0x0000001f;
            uint dp = 0x20 - dn;
            uint dq = ac >> (byte) dp;
            uint dr = ac << (byte) dn;
            uint ds = dl << 0x8;
            uint dt = ds ^ dm;
            uint du = cq + dt;
            uint dv = dq | dr;
            uint dw = dv ^ dt;
            byte dx = (byte) (du >> 16);
            uint dy = _0x595814[dx];
            byte dz = (byte) (du >> 8);
            uint ea = _0x595c14[dz];
            uint eb = du >> 0x18;
            uint ec = dy ^ ea;
            uint ed = du & 0x000000ff;
            uint ee = _0x595414[eb];
            uint ef = _0x596014[ed];
            uint eg = ec ^ ee;
            uint eh = eg ^ ef;
            byte ei = (byte) (eh >> 16);
            uint ej = eh >> 0x18;
            uint ek = _0x594C14[ej];
            uint el = _0x595014[ei];
            uint em = ek << 0x8;
            byte en = (byte) (eh >> 8);
            uint eo = el ^ em;
            uint ep = eo << 0x8;
            uint eq = _0x594C14[en];
            uint er = eh & 0x000000ff;
            uint es = ep ^ eq;
            uint et = es << 0x8;
            uint eu = _0x595014[er];
            uint ev = et ^ eu;
            uint ew = dw + ev;
            uint ex = co & 0x0000001f;
            uint ey = 0x20 - ex;
            uint ez = bf >> (byte) ey;
            uint fa = bf << (byte) ex;
            byte fb = (byte) (ew >> 8);
            uint fc = ez | fa;
            byte fd = (byte) (ew >> 16);
            uint fe = fc ^ ev;
            uint ff = _0x595c14[fb];
            uint fg = _0x595814[fd];
            uint fh = ew & 0x000000ff;
            uint fi = ew >> 0x18;
            uint fj = fg ^ ff;
            uint fk = _0x595414[fi];
            uint fl = _0x596014[fh];
            uint fm = fj ^ fk;
            uint fn = fm ^ fl;
            uint fo = (byte) (fn >> 16);
            uint fp = fn >> 0x18;
            uint fq = _0x595014[fo];
            uint fr = _0x594C14[fp];
            uint fs = (byte) (fn >> 8);
            uint ft = fr << 0x8;
            uint fu = fq ^ ft;
            uint fv = _0x594C14[fs];
            uint fw = fu << 0x8;
            uint fx = fn & 0x000000ff;
            uint fy = fw ^ fv;
            uint fz = fy << 0x8;
            uint ga = _0x595014[fx];
            uint gb = fz ^ ga;
            uint gc = du & 0x0000001f;
            uint gd = 0x20 - gc;
            uint ge = fe + gb;
            uint gf = co >> (byte) gd;
            uint gg = co << (byte) gc;
            byte gh = (byte) (ge >> 16);
            byte gi = (byte) (ge >> 8);
            uint gj = _0x595814[gh];
            uint gk = gf | gg;
            uint gl = gk ^ gb;
            uint gm = _0x595c14[gi];
            uint gn = gj ^ gm;
            uint go = ge >> 0x18;
            uint gp = _0x595414[go];
            uint gq = ge & 0x000000ff;
            uint gr = gn ^ gp;
            uint gs = _0x596014[gq];
            uint gt = gr ^ gs;
            byte gu = (byte) (gt >> 16);
            uint gv = gt >> 0x18;
            uint gw = _0x594C14[gv];
            uint gx = _0x595014[gu];
            uint gy = gw << 0x8;
            byte gz = (byte) (gt >> 8);
            uint ha = gx ^ gy;
            uint hb = ha << 0x8;
            uint hc = hb ^ _0x594C14[gz];
            uint hd = hc << 0x8;
            uint he = gt & 0x000000ff;
            uint hf = ge & 0x0000001f;
            uint hg = _0x595014[he];
            uint hh = hd ^ hg;
            uint hi = ew & 0x0000001f;
            uint hj = 0x20 - hi;
            uint hk = gl + hh;
            uint hl = du >> (byte) hj;
            uint hm = du << (byte) hi;
            byte hn = (byte) (hk >> 8);
            uint ho = hl | hm;
            byte hp = (byte) (hk >> 16);
            uint hq = ho ^ hh;
            uint hr = _0x595c14[hn];
            uint hs = _0x595814[hp];
            uint ht = hk >> 0x18;
            uint hu = hs ^ hr;
            uint hv = _0x595414[ht];
            uint hw = hk & 0x000000ff;
            uint hx = hu ^ hv;
            uint hy = _0x596014[hw];
            uint hz = hx ^ hy;
            byte ia = (byte) (hz >> 16);
            uint ib = hz >> 0x18;
            uint ic = _0x594C14[ib];
            uint id = ic << 0x8;
            uint ie = _0x595014[ia];
            uint ig = ie ^ id;
            byte ih = (byte) (hz >> 8);
            uint ii = hz & 0x000000ff;
            uint ij = ig << 0x8;
            uint ik = ij ^ _0x594C14[ih];
            uint il = _0x595014[ii];
            uint im = ik << 0x8;
            uint io = im ^ il;
            uint ip = 0x20 - hf;
            uint iq = hq + io;
            uint ir = ew >> (byte) ip;
            uint it = ew << (byte) hf;
            uint iu = ir | it;
            uint iv = iu ^ io;
            uint iy = hk & 0x0000001f;
            uint iz = 0x20 - iy;
            uint ja = ge >> (byte) iz;
            uint jb = ge << (byte) iy;
            uint jc = ja | jb;
            uint jd = jc - keyState.Hash[67];
            uint je = jd << (byte) iz;
            uint jf = jd >> (byte) iy;
            uint jg = je | jf;
            uint jh = iv ^ keyState.Hash[66];
            uint ji = iq & 0x0000001f;
            uint jj = 0x20 - ji;
            uint jk = jh << (byte) jj;
            uint jl = jh >> (byte) ji;
            uint jm = jg + jg;
            uint jn = jl | jk;
            uint jo = ~jn + 1;
            uint jp = jo - jm;
            uint jq = jg + jn;
            uint jr = hk + jp;
            uint js = keyState.Hash[63];
            uint jt = iq ^ jq;
            uint ju = keyState.Hash[64];
            uint jv = jn - jr;
            uint jw = jg ^ jt;
            uint jx = jv - js;
            uint jy = jr ^ ju;
            uint jz = jw & 0x0000001f;
            uint ka = 0x20 - jz;
            uint kb = jx << (byte) ka;
            uint kc = jx >> (byte) jz;
            uint kd = keyState.Hash[65];
            uint ke = jt - kd;
            uint kf = kb | kc;
            uint kg = keyState.Hash[62];
            uint kh = ke ^ kg;
            uint ki = jy & 0x0000001f;
            uint kj = 0x20 - ki;
            uint kk = kh << (byte) kj;
            uint kl = kh >> (byte) ki;
            uint km = kf + kf;
            uint kn = kk | kl;
            uint ko = ~kn + 1;
            uint kp = ko - km;
            uint kq = kf + kn;
            uint kr = jw + kp;
            uint ks = keyState.Hash[59];
            uint kt = jy ^ kq;
            uint ku = keyState.Hash[60];
            uint kv = kn - kr;
            uint kw = kf ^ kt;
            uint kx = kv - ks;
            uint ky = kr ^ ku;
            uint kz = kw & 0x0000001f;
            uint la = 0x20 - kz;
            uint lb = kx << (byte) la;
            uint lc = kx >> (byte) kz;
            uint ld = kt - keyState.Hash[61];
            uint le = lb | lc;
            uint lf = ld ^ keyState.Hash[58];
            uint lg = ky & 0x0000001f;
            uint lh = 0x20 - lg;
            uint li = lf << (byte) lh;
            uint lj = lf >> (byte) lg;
            uint lk = li | lj;
            uint ll = le + le;
            uint lm = ~lk + 1;
            uint ln = lm - ll;
            uint lo = le + lk;
            uint lp = kw + ln;
            uint lq = keyState.Hash[56];
            uint lr = ky ^ lo;
            uint ls = keyState.Hash[55];
            uint lt = lk - lp;
            uint lu = le ^ lr;
            uint lv = lt - ls;
            uint lw = lp ^ lq;
            uint lx = lu & 0x0000001f;
            uint ly = 0x20 - lx;
            uint lz = lv << (byte) ly;
            uint ma = lv >> (byte) lx;
            uint mb = keyState.Hash[57];
            uint mc = lr - mb;
            uint md = lz | ma;
            uint me = keyState.Hash[54];
            uint mf = mc ^ me;
            uint mg = lw & 0x0000001f;
            uint mh = 0x20 - mg;
            uint mi = mf << (byte) mh;
            uint mj = mf >> (byte) mg;
            uint mk = md + md;
            uint ml = mi | mj;
            uint mm = ~ml + 1;
            uint mn = mm - mk;
            uint mo = md + ml;
            uint mp = lu + mn;
            uint mq = keyState.Hash[51];
            uint mr = lw ^ mo;
            uint ms = keyState.Hash[52];
            uint mt = ml - mp;
            uint mu = md ^ mr;
            uint mv = mt - mq;
            uint mw = mp ^ ms;
            uint mx = mu & 0x0000001f;
            uint my = 0x20 - mx;
            uint mz = mv << (byte) my;
            uint na = mv >> (byte) mx;
            uint nb = keyState.Hash[53];
            uint nc = mr - nb;
            uint nd = mz | na;
            uint ne = keyState.Hash[50];
            uint nf = nc ^ ne;
            uint ng = mw & 0x0000001f;
            uint nh = 0x20 - ng;
            uint ni = nf << (byte) nh;
            uint nj = nf >> (byte) ng;
            uint nk = ni | nj;
            uint nl = nd + nd;
            uint nm = ~nk + 1;
            uint nn = nm - nl;
            uint no = nd + nk;
            uint np = mu + nn;
            uint nq = keyState.Hash[47];
            uint nr = mw ^ no;
            uint ns = keyState.Hash[48];
            uint nt = nk - np;
            uint nu = nd ^ nr;
            uint nv = nt - nq;
            uint nw = np ^ ns;
            uint nx = nu & 0x0000001f;
            uint ny = 0x20 - nx;
            uint nz = nv << (byte) ny;
            uint oa = nv >> (byte) nx;
            uint ob = keyState.Hash[49];
            uint oc = nr - ob;
            uint od = nz | oa;
            uint oe = keyState.Hash[46];
            uint of = oc ^ oe;
            uint og = nw & 0x0000001f;
            uint oh = 0x20 - og;
            uint oi = of << (byte) oh;
            uint oj = of >> (byte) og;
            uint ok = od + od;
            uint ol = oi | oj;
            uint om = ~ol + 1;
            uint on = om - ok;
            uint oo = od + ol;
            uint op = nu + on;
            uint oq = keyState.Hash[43];
            uint or = nw ^ oo;
            uint os = keyState.Hash[44];
            uint ot = ol - op;
            uint ou = od ^ or;
            uint ov = ot - oq;
            uint ow = op ^ os;
            uint ox = ou & 0x0000001f;
            uint oy = 0x20 - ox;
            uint oz = ov << (byte) oy;
            uint pa = ov >> (byte) ox;
            uint pb = keyState.Hash[45];
            uint pc = or - pb;
            uint pd = oz | pa;
            uint pe = keyState.Hash[42];
            uint pf = pc ^ pe;
            uint pg = ow & 0x0000001f;
            uint ph = 0x20 - pg;
            uint pi = pf << (byte) ph;
            uint pj = pf >> (byte) pg;
            uint pk = pi | pj;
            uint pl = pd + pd;
            uint pm = ~pk + 1;
            uint pn = pm - pl;
            uint po = pd + pk;
            uint pp = ou + pn;
            uint pq = ow ^ po;
            uint pr = keyState.Hash[39];
            uint ps = keyState.Hash[40];
            uint pt = pk - pp;
            uint pu = pd ^ pq;
            uint pv = pt - pr;
            uint pw = pp ^ ps;
            uint px = pu & 0x0000001f;
            uint py = 0x20 - px;
            uint pz = pv << (byte) py;
            uint qa = pv >> (byte) px;
            uint qb = keyState.Hash[41];
            uint qc = pq - qb;
            uint qd = pz | qa;
            uint qe = keyState.Hash[38];
            uint qf = qc ^ qe;
            uint qg = pw & 0x0000001f;
            uint qh = 0x20 - qg;
            uint qi = qf << (byte) qh;
            uint qj = qf >> (byte) qg;
            uint qk = qd + qd;
            uint ql = qi | qj;
            uint qm = ~ql + 1;
            uint qn = qm - qk;
            uint qo = qd + ql;
            uint qp = pu + qn;
            uint qq = keyState.Hash[35];
            uint qr = pw ^ qo;
            uint qs = keyState.Hash[36];
            uint qt = ql - qp;
            uint qu = qd ^ qr;
            uint qv = qt - qq;
            uint qw = qp ^ qs;
            uint qx = qu & 0x0000001f;
            uint qy = 0x20 - qx;
            uint qz = qv << (byte) qy;
            uint ra = qv >> (byte) qx;
            uint rb = keyState.Hash[37];
            uint rc = qr - rb;
            uint rd = qz | ra;
            uint re = keyState.Hash[34];
            uint rf = rc ^ re;
            uint rg = qw & 0x0000001f;
            uint rh = 0x20 - rg;
            uint ri = rf << (byte) rh;
            uint rj = rf >> (byte) rg;
            uint rk = ri | rj;
            uint rl = rd + rd;
            uint rm = ~rk + 1;
            uint rn = rm - rl;
            uint ro = rd + rk;
            uint rp = qu + rn;
            uint rq = qw ^ ro;
            uint rr = keyState.Hash[31];
            uint rs = keyState.Hash[32];
            uint rt = rk - rp;
            uint ru = rd ^ rq;
            uint rv = rt - rr;
            uint rw = rp ^ rs;
            uint rx = ru & 0x0000001f;
            uint ry = 0x20 - rx;
            uint rz = rv << (byte) ry;
            uint sa = rv >> (byte) rx;
            uint sb = keyState.Hash[33];
            uint sc = rq - sb;
            uint sd = rz | sa;
            uint se = keyState.Hash[30];
            uint sf = sc ^ se;
            uint sg = rw & 0x0000001f;
            uint sh = 0x20 - sg;
            uint si = sf << (byte) sh;
            uint sj = sf >> (byte) sg;
            uint sk = sd + sd;
            uint sl = si | sj;
            uint sm = ~sl + 1;
            uint sn = sm - sk;
            uint so = sd + sl;
            uint sp = ru + sn;
            uint sq = keyState.Hash[27];
            uint sr = rw ^ so;
            uint ss = keyState.Hash[28];
            uint st = sl - sp;
            uint su = sd ^ sr;
            uint sv = st - sq;
            uint sw = sp ^ ss;
            uint sx = su & 0x0000001f;
            uint sy = 0x20 - sx;
            uint sz = sv << (byte) sy;
            uint ta = sv >> (byte) sx;
            uint tb = keyState.Hash[29];
            uint tc = sr - tb;
            uint td = sz | ta;
            uint te = keyState.Hash[26];
            uint tf = tc ^ te;
            uint tg = sw & 0x0000001f;
            uint th = 0x20 - tg;
            uint ti = tf << (byte) th;
            uint tj = tf >> (byte) tg;
            uint tk = ti | tj;
            uint tl = td + td;
            uint tm = ~tk + 1;
            uint tn = tm - tl;
            uint to = td + tk;
            uint tp = su + tn;
            uint tq = sw ^ to;
            uint tr = keyState.Hash[23];
            uint ts = keyState.Hash[24];
            uint tt = tk - tp;
            uint tu = td ^ tq;
            uint tv = tt - tr;
            uint tw = tp ^ ts;
            uint tx = tu & 0x0000001f;
            uint ty = 0x20 - tx;
            uint tz = tv << (byte) ty;
            uint ua = tv >> (byte) tx;
            uint ub = keyState.Hash[25];
            uint uc = tq - ub;
            uint ud = tz | ua;
            uint ue = keyState.Hash[22];
            uint uf = uc ^ ue;
            uint ug = tw & 0x0000001f;
            uint uh = 0x20 - ug;
            uint ui = uf << (byte) uh;
            uint uj = uf >> (byte) ug;
            uint uk = ud + ud;
            uint ul = ui | uj;
            uint um = ~ul + 1;
            uint un = um - uk;
            uint uo = ud + ul;
            uint up = tu + un;
            uint uq = keyState.Hash[19];
            uint ur = tw ^ uo;
            uint us = keyState.Hash[20];
            uint ut = ul - up;
            uint uu = ud ^ ur;
            uint uv = ut - uq;
            uint uw = up ^ us;
            uint ux = uu & 0x0000001f;
            uint uy = 0x20 - ux;
            uint uz = uv << (byte) uy;
            uint va = uv >> (byte) ux;
            uint vb = keyState.Hash[21];
            uint vc = ur - vb;
            uint vd = uz | va;
            uint ve = keyState.Hash[18];
            uint vf = vc ^ ve;
            uint vg = uw & 0x0000001f;
            uint vh = 0x20 - vg;
            uint vi = vf << (byte) vh;
            uint vj = vf >> (byte) vg;
            uint vk = vi | vj;
            uint vl = vd + vd;
            uint vm = ~vk + 1;
            uint vn = vm - vl;
            uint vo = vd + vk;
            uint vp = uu + vn;
            uint vq = uw ^ vo;
            uint vr = keyState.Hash[15];
            uint vs = keyState.Hash[16];
            uint vt = vk - vp;
            uint vu = vd ^ vq;
            uint vv = vt - vr;
            uint vw = vp ^ vs;
            uint vx = vu & 0x0000001f;
            uint vy = 0x20 - vx;
            uint vz = vv << (byte) vy;
            uint wa = vv >> (byte) vx;
            uint wb = keyState.Hash[17];
            uint wc = vq - wb;
            uint wd = vz | wa;
            uint we = keyState.Hash[14];
            uint wf = wc ^ we;
            uint wg = vw & 0x0000001f;
            uint wh = 0x20 - wg;
            uint wi = wf << (byte) wh;
            uint wj = wf >> (byte) wg;
            uint wk = wd + wd;
            uint wl = wi | wj;
            uint wm = ~wl + 1;
            uint wn = wm - wk;
            uint wo = wd + wl;
            uint wp = vu + wn;
            uint wq = keyState.Hash[11];
            uint wr = vw ^ wo;
            uint ws = keyState.Hash[12];
            uint wt = wl - wp;
            uint wu = wd ^ wr;
            uint wv = wt - wq;
            uint ww = wp ^ ws;
            uint wx = wu & 0x0000001f;
            uint wy = 0x20 - wx;
            uint wz = wv << (byte) wy;
            uint xa = wv >> (byte) wx;
            uint xb = keyState.Hash[13];
            uint xc = wr - xb;
            uint xd = wz | xa;
            uint xe = keyState.Hash[10];
            uint xf = xc ^ xe;
            uint xg = ww & 0x0000001f;
            uint xh = 0x20 - xg;
            uint xi = xf << (byte) xh;
            uint xj = xf >> (byte) xg;
            uint xk = xi | xj;
            uint xl = xd + xd;
            uint xm = ~xk + 1;
            uint xn = xm - xl;
            uint xo = xd + xk;
            uint xp = wu + xn;
            uint xq = ww ^ xo;
            uint xr = keyState.Hash[7];
            uint xs = keyState.Hash[8];
            uint xt = xk - xp;
            uint xu = xd ^ xq;
            uint xv = xt - xr;
            uint xw = xp ^ xs;
            uint xx = xu & 0x0000001f;
            uint xy = 0x20 - xx;
            uint xz = xv << (byte) xy;
            uint ya = xv >> (byte) xx;
            uint yb = keyState.Hash[9];
            uint yc = xq - yb;
            uint yd = xz | ya;
            uint ye = keyState.Hash[6];
            uint yf = yc ^ ye;
            uint yg = xw & 0x0000001f;
            uint yh = 0x20 - yg;
            uint yi = yf << (byte) yh;
            uint yj = yf >> (byte) yg;
            uint yk = yd + yd;
            uint yl = yi | yj;
            uint ym = ~yl + 1;
            uint yn = ym - yk;
            uint yo = yd + yl;
            uint yp = xw ^ yo;
            uint yq = xu + yn;
            uint yr = yd ^ yp;
            uint ys = yl - yq;
            byte yt = (byte) (yr >> 8);
            byte yu = (byte) (yr >> 16);
            uint yv = _0x595814[yu];
            uint yw = _0x595c14[yt];
            uint yx = yv ^ yw;
            uint yy = yr >> 0x18;
            uint yz = _0x595414[yy];
            uint za = yr & 0x000000ff;
            uint zb = yx ^ yz;
            uint zc = _0x596014[za];
            uint zd = zb ^ zc;
            byte ze = (byte) (zd >> 16);
            uint zf = zd >> 0x18;
            uint zg = _0x595014[ze];
            uint zh = _0x594C14[zf];
            byte zi = (byte) (zd >> 8);
            uint zj = zh << 0x8;
            uint zk = zg ^ zj;
            uint zl = _0x594C14[zi];
            uint zm = zk << 0x8;
            uint zn = zd & 0x000000ff;
            uint zo = zm ^ zl;
            uint zp = zo << 0x8;
            uint zq = _0x595014[zn];
            uint zr = zp ^ zq;
            uint zs = yr & 0x0000001f;
            uint zt = 0x20 - zs;
            uint zu = ys << (byte) zt;
            uint zv = ys >> (byte) zs;
            uint zw = zu | zv;
            uint zx = yp - keyState.Hash[5];
            byte zz = (byte) (zw >> 16);
            uint aaa = zx ^ zr;
            uint aac = _0x595814[zz];
            uint aad = zw >> 0x18;
            uint aae = keyState.Hash[4];
            uint aaf = aae ^ yq;
            uint aag = aaf - zr;
            byte aah = (byte) (zw >> 8);
            uint aai = _0x595c14[aah];
            uint aaj = aac ^ aai;
            uint aak = _0x595414[aad];
            uint aal = zw & 0x000000ff;
            uint aam = aaj ^ aak;
            uint aan = _0x596014[aal];
            uint _wz = zw & 0x0000001f;
            uint _xb = aam ^ aan;
            byte _xc = (byte) (_xb >> 16);
            uint _xd = _xb >> 0x18;
            uint _xe = _0x594C14[_xd];
            uint _xf = _xe << 0x8;
            uint _xg = _0x595014[_xc];
            uint _xh = _xg ^ _xf;
            byte _xi = (byte) (_xb >> 8);
            uint _xj = _xb & 0x000000ff;
            uint _xk = _xh << 0x8;
            uint _xl = _xk ^ _0x594C14[_xi];
            uint _xm = _0x595014[_xj];
            uint _xn = _xl << 0x8;
            uint _xo = _xn ^ _xm;
            uint _xp = 0x20 - _wz;
            uint _xq = yr - _xo;
            uint _xr = aaa << (byte) _xp;
            uint _xs = aaa >> (byte) _wz;
            uint _xt = _xr | _xs;
            byte _xu = (byte) (_xt >> 8);
            byte _xv = (byte) (_xt >> 16);
            uint _xw = aag ^ _xo;
            uint _xx = _0x595c14[_xu];
            uint _xy = _0x595814[_xv];
            uint _xz = _xy ^ _xx;
            uint _ya = _xt >> 0x18;
            uint _yb = _0x595414[_ya];
            uint _yc = _xz ^ _yb;
            uint _yd = _xt & 0x000000ff;
            uint _ye = _0x596014[_yd];
            uint _yf = _yc ^ _ye;
            byte _yg = (byte) (_yf >> 16);
            uint _yh = _yf >> 0x18;
            uint _yi = _0x594C14[_yh];
            uint _yj = _yi << 0x8;
            uint _yk = _0x595014[_yg];
            uint _yl = _yk ^ _yj;
            byte _ym = (byte) (_yf >> 8);
            uint _yn = _yf & 0x000000ff;
            uint _yo = _yl << 0x8;
            uint _yp = _yo ^ _0x594C14[_ym];
            uint _yq = _0x595014[_yn];
            uint _yr = _yp << 0x8;
            uint _ys = _yr ^ _yq;
            uint _yt = _xt & 0x0000001f;
            uint _yu = 0x20 - _yt;
            uint _yv = _xq ^ _ys;
            uint _yw = _xw << (byte) _yu;
            uint _yx = _xw >> (byte) _yt;
            uint _yy = zw - _ys;
            uint _yz = _yw | _yx;
            byte _za = (byte) (_yz >> 8);
            byte _zb = (byte) (_yz >> 16);
            uint _zc = _0x595c14[_za];
            uint _zd = _0x595814[_zb];
            uint _ze = _yz >> 0x18;
            uint _zf = _zd ^ _zc;
            uint _zg = _0x595414[_ze];
            uint _zh = _yz & 0x000000ff;
            uint _zi = _zf ^ _zg;
            uint _zj = _0x596014[_zh];
            uint _zk = _zi ^ _zj;
            byte _zl = (byte) (_zk >> 16);
            uint _zm = _zk >> 0x18;
            uint _zn = _0x594C14[_zm];
            uint _zo = _0x595014[_zl];
            uint _zp = _zn << 0x8;
            uint _zq = _zo ^ _zp;
            byte _zr = (byte) (_zk >> 8);
            uint _zs = _zk & 0x000000ff;
            uint _zt = _zq << 0x8;
            uint _zu = _0x594C14[_zr];
            uint _zv = _0x595014[_zs];
            uint _zw = _zt ^ _zu;
            uint _zx = _zw << 0x8;
            uint _zy = _yz & 0x0000001f;
            uint _zz = _zx ^ _zv;
            uint _aaa = 0x20 - _zy;
            uint _aab = _yv << (byte) _aaa;
            uint _aac = _yv >> (byte) _zy;
            uint _aad = _yy ^ _zz;
            uint _aae = _aac | _aab;
            byte _aaf = (byte) (_aae >> 8);
            uint _aag = _xt - _zz;
            byte _aah = (byte) (_aae >> 16);
            uint _aai = _0x595c14[_aaf];
            uint _aaj = _0x595814[_aah];
            uint _aak = _aae >> 0x18;
            uint _aal = _aaj ^ _aai;
            uint _aam = _0x595414[_aak];
            uint _aan = _aae & 0x000000ff;
            uint _aao = _aal ^ _aam;
            uint _aap = _aae & 0x0000001f;
            uint _aaq = _0x596014[_aan];
            uint _aar = _aao ^ _aaq;
            byte _aas = (byte) (_aar >> 16);
            uint _aat = _aar >> 0x18;
            uint _aau = _0x594C14[_aat];
            uint _aav = _aau << 0x8;
            uint _aaw = _0x595014[_aas];
            uint _aax = _aaw ^ _aav;
            byte _aay = (byte) (_aar >> 8);
            uint _aaz = _aar & 0x000000ff;
            uint aba = _aax << 0x8;
            uint abc = aba ^ _0x594C14[_aay];
            uint abd = _0x595014[_aaz];
            uint abe = abc << 0x8;
            uint abf = abe ^ abd;
            uint abg = 0x20 - _aap;
            uint abh = _aad << (byte) abg;
            uint abi = _aag ^ abf;
            uint abj = _aad >> (byte) _aap;
            uint abk = _yz - abf;
            uint abl = abh | abj;
            byte abm = (byte) (abl >> 8);
            byte abn = (byte) (abl >> 16);
            uint abo = _0x595c14[abm];
            uint abp = _0x595814[abn];
            uint abq = abl >> 0x18;
            uint abr = abp ^ abo;
            uint abs = _0x595414[abq];
            uint abt = abl & 0x000000ff;
            uint abu = abr ^ abs;
            uint abv = _0x596014[abt];
            uint abw = abu ^ abv;
            byte abx = (byte) (abw >> 16);
            uint aby = abw >> 0x18;
            uint abz = _0x594C14[aby];
            uint aca = abz << 0x8;
            uint acb = _0x595014[abx];
            uint acc = aca ^ acb;
            byte acd = (byte) (abw >> 8);
            uint ace = abw & 0x000000ff;
            uint acf = acc << 0x8;
            uint acg = acf ^ _0x594C14[acd];
            uint ach = _0x595014[ace];
            uint aci = acg << 0x8;
            uint acj = aci ^ ach;
            uint ack = abl & 0x0000001f;
            uint acl = abk ^ acj;
            uint acm = 0x20 - ack;
            uint acn = abi << (byte) acm;
            uint aco = abi >> (byte) ack;
            uint acp = _aae - acj;
            uint acq = acn | aco;
            byte acr = (byte) (acq >> 8);
            byte acs = (byte) (acq >> 16);
            uint act = _0x595814[acs];
            uint acu = _0x595c14[acr];
            uint acv = act ^ acu;
            uint acw = acq >> 0x18;
            uint acx = _0x595414[acw];
            uint acy = acv ^ acx;
            uint acz = acq & 0x000000ff;
            uint ada = _0x596014[acz];
            uint adb = acy ^ ada;
            byte adc = (byte) (adb >> 16);
            uint add = adb >> 0x18;
            uint ade = _0x594C14[add];
            uint adf = _0x595014[adc];
            uint adg = ade << 0x8;
            uint adh = adf ^ adg;
            uint adi = (byte) (adb >> 8);
            uint adj = adb & 0x000000ff;
            uint adk = adh << 0x8;
            uint adl = _0x594C14[adi];
            uint adm = adk ^ adl;
            uint adn = _0x595014[adj];
            uint ado = adm << 0x8;
            uint adp = ado ^ adn;
            uint adq = acq & 0x0000001f;
            uint adr = 0x20 - adq;
            uint ads = acl << (byte) adr;
            uint adt = acl >> (byte) adq;
            uint adu = acp ^ adp;
            uint adv = abl - adp;
            uint adw = ads | adt;
            byte adx = (byte) (adw >> 8);
            byte ady = (byte) (adw >> 16);
            uint adz = _0x595c14[adx];
            uint aea = _0x595814[ady];
            uint aeb = adz ^ aea;
            uint aec = adw >> 0x18;
            uint aed = adw & 0x000000ff;
            uint aee = _0x595414[aec];
            uint aef = _0x596014[aed];
            uint aeg = aeb ^ aee;
            uint aeh = aeg ^ aef;
            byte aei = (byte) (aeh >> 16);
            uint aej = aeh >> 0x18;
            uint aek = _0x595014[aei];
            uint ael = _0x594C14[aej];
            uint aem = ael << 0x8;
            uint aen = aek ^ aem;
            byte aeo = (byte) (aeh >> 8);
            uint aep = aeh & 0x000000ff;
            uint aeq = aen << 0x8;
            uint aer = _0x594C14[aeo];
            uint aes = keyState.Hash[0];
            uint aet = aeq ^ aer;
            uint aeu = _0x595014[aep];
            uint aev = aet << 0x8;
            uint aew = aev ^ aeu;
            uint aex = adw - aes;
            uint aey = acq - aew;
            uint aez = aey ^ keyState.Hash[1];
            uint afa = keyState.Hash[2];
            uint afb = aew ^ adv;
            uint afc = afb - afa;
            uint afd = adw & 0x0000001f;
            uint afe = 0x20 - afd;
            uint aff = adu << (byte) afe;
            uint afg = keyState.Hash[3];
            uint afh = adu >> (byte) afd;
            uint afi = aff | afh;
            uint afj = afi ^ afg;
            keyState.Output[0] = aex;
            keyState.Output[1] = aez;
            keyState.Output[2] = afc;
            keyState.Output[3] = afj;
        }

        private void fun_4d7030(KeyState keyState)
        {
            uint a = keyState.Init[0] + keyState.Hash[0];
            uint b = keyState.Init[3] ^ keyState.Hash[3];
            byte c = (byte) (a >> 8);
            byte d = (byte) (a >> 16);
            uint e = _0x595c14[c];
            uint f = a & 0x000000ff;
            uint g = _0x595814[d];
            uint h = a >> 0x18;
            uint i = g ^ e;
            uint j = _0x595414[h];
            uint k = _0x596014[f];
            uint l = i ^ j;
            uint m = l ^ k;
            byte n = (byte) (m >> 16);
            uint o = m >> 0x18;
            uint p = _0x595014[n];
            uint q = _0x594C14[o];
            uint r = q << 0x8;
            uint s = p ^ r;
            byte t = (byte) (m >> 8);
            uint u = m & 0x000000ff;
            uint v = s << 0x8;
            uint w = _0x594C14[t];
            uint x = _0x595014[u];
            uint y = v ^ w;
            uint z = y << 0x8;
            uint aa = z ^ x;
            uint ab = keyState.Init[1] ^ keyState.Hash[1];
            uint ac = aa + ab;
            uint ad = keyState.Init[2] + keyState.Hash[2];
            uint ae = ad ^ aa;
            byte af = (byte) (ac >> 16);
            byte ag = (byte) (ac >> 8);
            uint ah = _0x595814[af];
            uint ai = _0x595c14[ag];
            uint aj = ah ^ ai;
            uint ak = ac >> 0x18;
            uint al = _0x595414[ak];
            uint am = ac & 0x000000ff;
            uint an = aj ^ al;
            uint ao = _0x596014[am];
            uint ap = an ^ ao;
            byte aq = (byte) (ap >> 16);
            uint ar = ap >> 0x18;
            uint at = _0x594C14[ar];
            uint au = at << 0x8;
            uint av = _0x595014[aq];
            uint aw = av ^ au;
            byte ax = (byte) (ap >> 8);
            uint ay = ap & 0x000000ff;
            uint az = aw << 0x8;
            uint ba = az ^ _0x594C14[ax];
            uint bb = _0x595014[ay];
            uint bc = ba << 0x8;
            uint bd = bc ^ bb;
            uint be = a & 0x0000001f;
            uint bf = ae + bd;
            uint bg = 0x20 - be;
            uint bh = b >> (byte) bg;
            uint bi = ac & 0x0000001f;
            uint bj = b << (byte) be;
            uint bk = bh | bj;
            byte bl = (byte) (bf >> 8);
            uint bm = bk ^ bd;
            uint bn = _0x595c14[bl];
            uint bo = (byte) (bf >> 16);
            uint bp = _0x595814[bo];
            uint bq = bp ^ bn;
            uint br = bf >> 0x18;
            uint bs = _0x595414[br];
            uint bt = bf & 0x000000ff;
            uint bu = bq ^ bs;
            uint bv = _0x596014[bt];
            uint bw = bu ^ bv;
            uint bx = (byte) (bw >> 16);
            uint by = bw >> 0x18;
            uint bz = _0x594C14[by];
            uint ca = _0x595014[bx];
            uint cb = bz << 0x8;
            uint cc = (byte) (bw >> 8);
            uint cd = bw & 0x000000ff;
            uint ce = ca ^ cb;
            uint cf = _0x594C14[cc];
            uint cg = _0x595014[cd];
            uint ch = ce << 0x8;
            uint ci = 0x20 - bi;
            uint cj = ch ^ cf;
            uint ck = a >> (byte) ci;
            uint cl = cj << 0x8;
            uint cm = a << (byte) bi;
            uint cn = cl ^ cg;
            uint co = bm + cn;
            uint cp = ck | cm;
            uint cq = cp ^ cn;
            byte cr = (byte) (co >> 16);
            uint cs = _0x595814[cr];
            byte ct = (byte) (co >> 8);
            uint cu = cs ^ _0x595c14[ct];
            uint cv = co & 0x000000ff;
            uint cw = co >> 0x18;
            uint cx = _0x596014[cv];
            uint cy = _0x595414[cw];
            uint cz = cu ^ cy;
            uint da = cz ^ cx;
            byte db = (byte) (da >> 16);
            uint dc = da >> 0x18;
            uint dd = _0x594C14[dc];
            uint de = _0x595014[db];
            uint df = dd << 0x8;
            byte dg = (byte) (da >> 8);
            uint dh = de ^ df;
            uint di = dh << 0x8;
            uint dj = _0x594C14[dg];
            uint dk = da & 0x000000ff;
            uint dl = di ^ dj;
            uint dm = _0x595014[dk];
            uint dn = bf & 0x0000001f;
            uint dp = 0x20 - dn;
            uint dq = ac >> (byte) dp;
            uint dr = ac << (byte) dn;
            uint ds = dl << 0x8;
            uint dt = ds ^ dm;
            uint du = cq + dt;
            uint dv = dq | dr;
            uint dw = dv ^ dt;
            byte dx = (byte) (du >> 16);
            uint dy = _0x595814[dx];
            byte dz = (byte) (du >> 8);
            uint ea = _0x595c14[dz];
            uint eb = du >> 0x18;
            uint ec = dy ^ ea;
            uint ed = du & 0x000000ff;
            uint ee = _0x595414[eb];
            uint ef = _0x596014[ed];
            uint eg = ec ^ ee;
            uint eh = eg ^ ef;
            byte ei = (byte) (eh >> 16);
            uint ej = eh >> 0x18;
            uint ek = _0x594C14[ej];
            uint el = _0x595014[ei];
            uint em = ek << 0x8;
            byte en = (byte) (eh >> 8);
            uint eo = el ^ em;
            uint ep = eo << 0x8;
            uint eq = _0x594C14[en];
            uint er = eh & 0x000000ff;
            uint es = ep ^ eq;
            uint et = es << 0x8;
            uint eu = _0x595014[er];
            uint ev = et ^ eu;
            uint ew = dw + ev;
            uint ex = co & 0x0000001f;
            uint ey = 0x20 - ex;
            uint ez = bf >> (byte) ey;
            uint fa = bf << (byte) ex;
            byte fb = (byte) (ew >> 8);
            uint fc = ez | fa;
            byte fd = (byte) (ew >> 16);
            uint fe = fc ^ ev;
            uint ff = _0x595c14[fb];
            uint fg = _0x595814[fd];
            uint fh = ew & 0x000000ff;
            uint fi = ew >> 0x18;
            uint fj = fg ^ ff;
            uint fk = _0x595414[fi];
            uint fl = _0x596014[fh];
            uint fm = fj ^ fk;
            uint fn = fm ^ fl;
            uint fo = (byte) (fn >> 16);
            uint fp = fn >> 0x18;
            uint fq = _0x595014[fo];
            uint fr = _0x594C14[fp];
            uint fs = (byte) (fn >> 8);
            uint ft = fr << 0x8;
            uint fu = fq ^ ft;
            uint fv = _0x594C14[fs];
            uint fw = fu << 0x8;
            uint fx = fn & 0x000000ff;
            uint fy = fw ^ fv;
            uint fz = fy << 0x8;
            uint ga = _0x595014[fx];
            uint gb = fz ^ ga;
            uint gc = du & 0x0000001f;
            uint gd = 0x20 - gc;
            uint ge = fe + gb;
            uint gf = co >> (byte) gd;
            uint gg = co << (byte) gc;
            byte gh = (byte) (ge >> 16);
            byte gi = (byte) (ge >> 8);
            uint gj = _0x595814[gh];
            uint gk = gf | gg;
            uint gl = gk ^ gb;
            uint gm = _0x595c14[gi];
            uint gn = gj ^ gm;
            uint go = ge >> 0x18;
            uint gp = _0x595414[go];
            uint gq = ge & 0x000000ff;
            uint gr = gn ^ gp;
            uint gs = _0x596014[gq];
            uint gt = gr ^ gs;
            byte gu = (byte) (gt >> 16);
            uint gv = gt >> 0x18;
            uint gw = _0x594C14[gv];
            uint gx = _0x595014[gu];
            uint gy = gw << 0x8;
            byte gz = (byte) (gt >> 8);
            uint ha = gx ^ gy;
            uint hb = ha << 0x8;
            uint hc = hb ^ _0x594C14[gz];
            uint hd = hc << 0x8;
            uint he = gt & 0x000000ff;
            uint hf = ge & 0x0000001f;
            uint hg = _0x595014[he];
            uint hh = hd ^ hg;
            uint hi = ew & 0x0000001f;
            uint hj = 0x20 - hi;
            uint hk = gl + hh;
            uint hl = du >> (byte) hj;
            uint hm = du << (byte) hi;
            byte hn = (byte) (hk >> 8);
            uint ho = hl | hm;
            byte hp = (byte) (hk >> 16);
            uint hq = ho ^ hh;
            uint hr = _0x595c14[hn];
            uint hs = _0x595814[hp];
            uint ht = hk >> 0x18;
            uint hu = hs ^ hr;
            uint hv = _0x595414[ht];
            uint hw = hk & 0x000000ff;
            uint hx = hu ^ hv;
            uint hy = _0x596014[hw];
            uint hz = hx ^ hy;
            byte ia = (byte) (hz >> 16);
            uint ib = hz >> 0x18;
            uint ic = _0x594C14[ib];
            uint id = ic << 0x8;
            uint ie = _0x595014[ia];
            uint ig = ie ^ id;
            byte ih = (byte) (hz >> 8);
            uint ii = hz & 0x000000ff;
            uint ij = ig << 0x8;
            uint ik = ij ^ _0x594C14[ih];
            uint il = _0x595014[ii];
            uint im = ik << 0x8;
            uint io = im ^ il;
            uint ip = 0x20 - hf;
            uint iq = hq + io;
            uint ir = ew >> (byte) ip;
            uint it = ew << (byte) hf;
            uint iu = ir | it;
            uint iv = iu ^ io;
            uint iw = iv + keyState.Hash[5];
            uint ix = iq ^ keyState.Hash[4];
            uint iy = hk & 0x0000001f;
            uint iz = 0x20 - iy;
            uint ja = ge >> (byte) iz;
            uint jb = ge << (byte) iy;
            uint jc = ja | jb;
            uint jd = hk ^ iw;
            uint je = jc + ix;
            uint jf = jd + je;
            uint jg = iw ^ jf;
            uint jh = jd * 2 + je;
            uint ji = ix + jh;
            uint jj = jg & 0x0000001f;
            uint jk = 0x20 - jj;
            uint jl = je >> (byte) jk;
            uint jm = je << (byte) jj;
            uint jn = jg ^ keyState.Hash[8];
            uint jo = jl | jm;
            uint jp = jo ^ keyState.Hash[6];
            uint jq = jp + keyState.Hash[9];
            uint jr = ji & 0x0000001f;
            uint js = ji ^ jq;
            uint jt = 0x20 - jr;
            uint ju = jd >> (byte) jt;
            uint jv = jd << (byte) jr;
            uint jw = ju | jv;
            uint jx = jw + keyState.Hash[7];
            uint jy = jx + jn;
            uint jz = js + jy;
            uint ka = js * 2 + jy;
            uint kb = jq ^ jz;
            uint kc = jn + ka;
            uint kd = kb & 0x0000001f;
            uint ke = 0x20 - kd;
            uint kf = jy >> (byte) ke;
            uint kg = jy << (byte) kd;
            uint kh = kb ^ keyState.Hash[12];
            uint ki = kf | kg;
            uint kj = ki ^ keyState.Hash[10];
            uint kk = kj + keyState.Hash[13];
            uint kl = kc & 0x0000001f;
            uint km = kc ^ kk;
            uint kn = 0x20 - kl;
            uint ko = js >> (byte) kn;
            uint kp = js << (byte) kl;
            uint kq = ko | kp;
            uint kr = kq + keyState.Hash[11];
            uint ks = kr + kh;
            uint kt = km + ks;
            uint ku = kk ^ kt;
            uint kv = km * 2 + ks;
            uint kw = kh + kv;
            uint kx = ku & 0x0000001f;
            uint ky = 0x20 - kx;
            uint kz = ks >> (byte) ky;
            uint la = ks << (byte) kx;
            uint lb = ku ^ keyState.Hash[16];
            uint lc = kz | la;
            uint ld = lc ^ keyState.Hash[14];
            uint le = ld + keyState.Hash[17];
            uint lf = kw & 0x0000001f;
            uint lg = kw ^ le;
            uint lh = 0x20 - lf;
            uint li = km >> (byte) lh;
            uint lj = km << (byte) lf;
            uint lk = li | lj;
            uint ll = lk + keyState.Hash[15];
            uint lm = ll + lb;
            uint ln = lm + lg;
            uint lo = lg * 2 + lm;
            uint lp = le ^ ln;
            uint lq = lb + lo;
            uint lr = lp & 0x0000001f;
            uint ls = 0x20 - lr;
            uint lt = lm >> (byte) ls;
            uint lu = lm << (byte) lr;
            uint lv = lp ^ keyState.Hash[20];
            uint lw = lt | lu;
            uint lx = lw ^ keyState.Hash[18];
            uint ly = lx + keyState.Hash[21];
            uint lz = lq & 0x0000001f;
            uint ma = lq ^ ly;
            uint mb = 0x20 - lz;
            uint mc = lg >> (byte) mb;
            uint md = lg << (byte) lz;
            uint me = mc | md;
            uint mf = me + keyState.Hash[19];
            uint mg = mf + lv;
            uint mh = mg + ma;
            uint mi = ly ^ mh;
            uint mj = ma * 2 + mg;
            uint mk = lv + mj;
            uint ml = mi & 0x0000001f;
            uint mm = 0x20 - ml;
            uint mn = mg >> (byte) mm;
            uint mo = mg << (byte) ml;
            uint mp = mi ^ keyState.Hash[24];
            uint mq = mn | mo;
            uint mr = mq ^ keyState.Hash[22];
            uint ms = mr + keyState.Hash[25];
            uint mt = mk & 0x0000001f;
            uint mu = mk ^ ms;
            uint mv = 0x20 - mt;
            uint mw = ma >> (byte) mv;
            uint mx = ma << (byte) mt;
            uint my = mw | mx;
            uint mz = my + keyState.Hash[23];
            uint na = mz + mp;
            uint nb = na + mu;
            uint nc = mu * 2 + na;
            uint nd = ms ^ nb;
            uint ne = mp + nc;
            uint nf = nd & 0x0000001f;
            uint ng = 0x20 - nf;
            uint nh = na >> (byte) ng;
            uint ni = na << (byte) nf;
            uint nj = nd ^ keyState.Hash[28];
            uint nk = nh | ni;
            uint nl = nk ^ keyState.Hash[26];
            uint nm = nl + keyState.Hash[29];
            uint nn = ne & 0x0000001f;
            uint no = ne ^ nm;
            uint np = 0x20 - nn;
            uint nq = mu >> (byte) np;
            uint nr = mu << (byte) nn;
            uint ns = nq | nr;
            uint nt = ns + keyState.Hash[27];
            uint nu = nt + nj;
            uint nv = nu + no;
            uint nw = nm ^ nv;
            uint nx = no * 2 + nu;
            uint ny = nj + nx;
            uint nz = nw & 0x0000001f;
            uint oa = 0x20 - nz;
            uint ob = nu >> (byte) oa;
            uint oc = nu << (byte) nz;
            uint od = nw ^ keyState.Hash[32];
            uint oe = ob | oc;
            uint of = oe ^ keyState.Hash[30];
            uint og = of + keyState.Hash[33];
            uint oh = ny & 0x0000001f;
            uint oi = ny ^ og;
            uint oj = 0x20 - oh;
            uint ok = no >> (byte) oj;
            uint ol = no << (byte) oh;
            uint om = ok | ol;
            uint on = om + keyState.Hash[31];
            uint oo = on + od;
            uint op = oo + oi;
            uint oq = og ^ op;
            uint or = oi * 2 + oo;
            uint os = od + or;
            uint ot = oq & 0x0000001f;
            uint ou = 0x20 - ot;
            uint ov = oo >> (byte) ou;
            uint ow = oo << (byte) ot;
            uint ox = oq ^ keyState.Hash[36];
            uint oy = ov | ow;
            uint oz = oy ^ keyState.Hash[34];
            uint pa = oz + keyState.Hash[37];
            uint pb = os & 0x0000001f;
            uint pc = os ^ pa;
            uint pd = 0x20 - pb;
            uint pe = oi >> (byte) pd;
            uint pf = oi << (byte) pb;
            uint pg = pe | pf;
            uint ph = pg + keyState.Hash[35];
            uint pi = ph + ox;
            uint pj = pi + pc;
            uint pk = pa ^ pj;
            uint pl = pc * 2 + pi;
            uint pm = ox + pl;
            uint pn = pk & 0x0000001f;
            uint po = 0x20 - pn;
            uint pp = pi >> (byte) po;
            uint pq = pi << (byte) pn;
            uint pr = pk ^ keyState.Hash[40];
            uint ps = pp | pq;
            uint pt = ps ^ keyState.Hash[38];
            uint pu = pt + keyState.Hash[41];
            uint pv = pm & 0x0000001f;
            uint pw = pm ^ pu;
            uint px = 0x20 - pv;
            uint py = pc >> (byte) px;
            uint pz = pc << (byte) pv;
            uint qa = py | pz;
            uint qb = qa + keyState.Hash[39];
            uint qc = qb + pr;
            uint qd = pw + qc;
            uint qe = qc + pw * 2;
            uint qf = pu ^ qd;
            uint qg = pr + qe;
            uint qh = qf & 0x0000001f;
            uint qi = 0x20 - qh;
            uint qj = qc >> (byte) qi;
            uint qk = qc << (byte) qh;
            uint ql = qj | qk;
            uint qm = keyState.Hash[42];
            uint qn = keyState.Hash[44];
            uint qo = ql ^ qm;
            uint qp = keyState.Hash[45];
            uint qq = qf ^ qn;
            uint qr = qo + qp;
            uint qs = qg & 0x0000001f;
            uint qt = 0x20 - qs;
            uint qu = qg ^ qr;
            uint qv = pw >> (byte) qt;
            uint qw = pw << (byte) qs;
            uint qx = qv | qw;
            uint qy = qx + keyState.Hash[43];
            uint qz = qy + qq;
            uint ra = qu + qz;
            uint rb = qr ^ ra;
            uint rc = qz + qu * 2;
            uint rd = qq + rc;
            uint re = rb & 0x0000001f;
            uint rf = 0x20 - re;
            uint rg = qz >> (byte) rf;
            uint rh = qz << (byte) re;
            uint ri = rb ^ keyState.Hash[48];
            uint rj = rg | rh;
            uint rk = rj ^ keyState.Hash[46];
            uint rl = rk + keyState.Hash[49];
            uint rm = rd & 0x0000001f;
            uint rn = rd ^ rl;
            uint ro = 0x20 - rm;
            uint rp = qu >> (byte) ro;
            uint rq = qu << (byte) rm;
            uint rr = rp | rq;
            uint rs = rr + keyState.Hash[47];
            uint rt = rs + ri;
            uint ru = rn + rt;
            uint rv = rt + rn * 2;
            uint rw = rl ^ ru;
            uint rx = ri + rv;
            uint ry = rw & 0x0000001f;
            uint rz = 0x20 - ry;
            uint sa = rt >> (byte) rz;
            uint sb = rt << (byte) ry;
            uint sc = rw ^ keyState.Hash[52];
            uint sd = sa | sb;
            uint se = sd ^ keyState.Hash[50];
            uint sf = se + keyState.Hash[53];
            uint sg = rx & 0x0000001f;
            uint sh = 0x20 - sg;
            uint si = rn >> (byte) sh;
            uint sj = rn << (byte) sg;
            uint sk = rx ^ sf;
            uint sl = si | sj;
            uint sm = sl + keyState.Hash[51];
            uint sn = sm + sc;
            uint so = sk + sn;
            uint sp = sf ^ so;
            uint sq = sn + sk * 2;
            uint sr = sc + sq;
            uint ss = sp & 0x0000001f;
            uint st = 0x20 - ss;
            uint su = sn >> (byte) st;
            uint sv = sn << (byte) ss;
            uint sw = sp ^ keyState.Hash[56];
            uint sx = su | sv;
            uint sy = sx ^ keyState.Hash[54];
            uint sz = sy + keyState.Hash[57];
            uint ta = sr & 0x0000001f;
            uint tb = sr ^ sz;
            uint tc = 0x20 - ta;
            uint td = sk >> (byte) tc;
            uint te = sk << (byte) ta;
            uint tf = td | te;
            uint tg = tf + keyState.Hash[55];
            uint th = tg + sw;
            uint ti = th + tb;
            uint tj = th + tb * 2;
            uint tk = sz ^ ti;
            uint tl = sw + tj;
            uint tm = tk & 0x0000001f;
            uint tn = 0x20 - tm;
            uint to = th >> (byte) tn;
            uint tp = th << (byte) tm;
            uint tq = tk ^ keyState.Hash[60];
            uint tr = to | tp;
            uint ts = tr ^ keyState.Hash[58];
            uint tt = ts + keyState.Hash[61];
            uint tu = tl & 0x0000001f;
            uint tv = tl ^ tt;
            uint tw = 0x20 - tu;
            uint tx = tb >> (byte) tw;
            uint ty = tb << (byte) tu;
            uint tz = tx | ty;
            uint ua = tz + keyState.Hash[59];
            uint ub = ua + tq;
            uint uc = ub + tv;
            uint ud = tt ^ uc;
            uint ue = ub + tv * 2;
            uint uf = tq + ue;
            uint ug = ud & 0x0000001f;
            uint uh = 0x20 - ug;
            uint ui = ub >> (byte) uh;
            uint uj = ub << (byte) ug;
            uint uk = ud ^ keyState.Hash[64];
            uint ul = ui | uj;
            uint um = ul ^ keyState.Hash[62];
            uint un = um + keyState.Hash[65];
            uint uo = uf & 0x0000001f;
            uint up = uf ^ un;
            uint uq = 0x20 - uo;
            uint ur = tv >> (byte) uq;
            uint us = tv << (byte) uo;
            uint ut = ur | us;
            uint uu = ut + keyState.Hash[63];
            uint uv = uu + uk;
            uint uw = up + uv;
            uint ux = un ^ uw;
            uint uy = uv + up * 2;
            uint uz = uk + uy;
            uint va = uz & 0x0000001f;
            uint vb = 0x20 - va;
            uint vc = up >> (byte) vb;
            uint vd = up << (byte) va;
            uint ve = vc | vd;
            uint vf = ve + keyState.Hash[67];
            byte vg = (byte) (uz >> 16);
            uint vh = _0x595814[vg];
            byte vi = (byte) (uz >> 8);
            uint vj = _0x595c14[vi];
            uint vk = vh ^ vj;
            uint vl = uz >> 0x18;
            uint vm = _0x595414[vl];
            uint vn = uz & 0x000000ff;
            uint vo = vk ^ vm;
            uint vp = _0x596014[vn];
            uint vq = vo ^ vp;
            byte vr = (byte) (vq >> 16);
            uint vs = vq >> 0x18;
            uint vt = _0x595014[vr];
            uint vu = _0x594C14[vs];
            byte vv = (byte) (vq >> 8);
            uint vw = vu << 0x8;
            uint vx = vt ^ vw;
            uint vy = _0x594C14[vv];
            uint vz = vx << 0x8;
            uint wa = vq & 0x000000ff;
            uint wb = vz ^ vy;
            uint wc = wb << 0x8;
            uint wd = _0x595014[wa];
            uint we = wc ^ wd;
            uint wf = vf << (byte) vb;
            uint wg = vf >> (byte) va;
            uint wh = wf | wg;
            uint wi = ux & 0x0000001f;
            uint wj = 0x20 - wi;
            uint wk = uv >> (byte) wj;
            uint wl = uv << (byte) wi;
            uint wm = wk | wl;
            uint wn = wm ^ keyState.Hash[66];
            uint wo = ux - we;
            byte wp = (byte) (wh >> 8);
            byte wq = (byte) (wh >> 16);
            uint wr = wn ^ we;
            uint ws = _0x595c14[wp];
            uint wt = _0x595814[wq];
            uint wu = wh >> 0x18;
            uint wv = wt ^ ws;
            uint ww = _0x595414[wu];
            uint wx = wh & 0x000000ff;
            uint wy = wv ^ ww;
            uint wz = wh & 0x0000001f;
            uint xa = _0x596014[wx];
            uint xb = wy ^ xa;
            byte xc = (byte) (xb >> 16);
            uint xd = xb >> 0x18;
            uint xe = _0x594C14[xd];
            uint xf = xe << 0x8;
            uint xg = _0x595014[xc];
            uint xh = xg ^ xf;
            byte xi = (byte) (xb >> 8);
            uint xj = xb & 0x000000ff;
            uint xk = xh << 0x8;
            uint xl = xk ^ _0x594C14[xi];
            uint xm = _0x595014[xj];
            uint xn = xl << 0x8;
            uint xo = xn ^ xm;
            uint xp = 0x20 - wz;
            uint xq = uz - xo;
            uint xr = wr << (byte) xp;
            uint xs = wr >> (byte) wz;
            uint xt = xr | xs;
            byte xu = (byte) (xt >> 8);
            byte xv = (byte) (xt >> 16);
            uint xw = wo ^ xo;
            uint xx = _0x595c14[xu];
            uint xy = _0x595814[xv];
            uint xz = xy ^ xx;
            uint ya = xt >> 0x18;
            uint yb = _0x595414[ya];
            uint yc = xz ^ yb;
            uint yd = xt & 0x000000ff;
            uint ye = _0x596014[yd];
            uint yf = yc ^ ye;
            byte yg = (byte) (yf >> 16);
            uint yh = yf >> 0x18;
            uint yi = _0x594C14[yh];
            uint yj = yi << 0x8;
            uint yk = _0x595014[yg];
            uint yl = yk ^ yj;
            byte ym = (byte) (yf >> 8);
            uint yn = yf & 0x000000ff;
            uint yo = yl << 0x8;
            uint yp = yo ^ _0x594C14[ym];
            uint yq = _0x595014[yn];
            uint yr = yp << 0x8;
            uint ys = yr ^ yq;
            uint yt = xt & 0x0000001f;
            uint yu = 0x20 - yt;
            uint yv = xq ^ ys;
            uint yw = xw << (byte) yu;
            uint yx = xw >> (byte) yt;
            uint yy = wh - ys;
            uint yz = yw | yx;
            byte za = (byte) (yz >> 8);
            byte zb = (byte) (yz >> 16);
            uint zc = _0x595c14[za];
            uint zd = _0x595814[zb];
            uint ze = yz >> 0x18;
            uint zf = zd ^ zc;
            uint zg = _0x595414[ze];
            uint zh = yz & 0x000000ff;
            uint zi = zf ^ zg;
            uint zj = _0x596014[zh];
            uint zk = zi ^ zj;
            byte zl = (byte) (zk >> 16);
            uint zm = zk >> 0x18;
            uint zn = _0x594C14[zm];
            uint zo = _0x595014[zl];
            uint zp = zn << 0x8;
            uint zq = zo ^ zp;
            byte zr = (byte) (zk >> 8);
            uint zs = zk & 0x000000ff;
            uint zt = zq << 0x8;
            uint zu = _0x594C14[zr];
            uint zv = _0x595014[zs];
            uint zw = zt ^ zu;
            uint zx = zw << 0x8;
            uint zy = yz & 0x0000001f;
            uint zz = zx ^ zv;
            uint aaa = 0x20 - zy;
            uint aab = yv << (byte) aaa;
            uint aac = yv >> (byte) zy;
            uint aad = yy ^ zz;
            uint aae = aac | aab;
            byte aaf = (byte) (aae >> 8);
            uint aag = xt - zz;
            byte aah = (byte) (aae >> 16);
            uint aai = _0x595c14[aaf];
            uint aaj = _0x595814[aah];
            uint aak = aae >> 0x18;
            uint aal = aaj ^ aai;
            uint aam = _0x595414[aak];
            uint aan = aae & 0x000000ff;
            uint aao = aal ^ aam;
            uint aap = aae & 0x0000001f;
            uint aaq = _0x596014[aan];
            uint aar = aao ^ aaq;
            byte aas = (byte) (aar >> 16);
            uint aat = aar >> 0x18;
            uint aau = _0x594C14[aat];
            uint aav = aau << 0x8;
            uint aaw = _0x595014[aas];
            uint aax = aaw ^ aav;
            byte aay = (byte) (aar >> 8);
            uint aaz = aar & 0x000000ff;
            uint aba = aax << 0x8;
            uint abc = aba ^ _0x594C14[aay];
            uint abd = _0x595014[aaz];
            uint abe = abc << 0x8;
            uint abf = abe ^ abd;
            uint abg = 0x20 - aap;
            uint abh = aad << (byte) abg;
            uint abi = aag ^ abf;
            uint abj = aad >> (byte) aap;
            uint abk = yz - abf;
            uint abl = abh | abj;
            byte abm = (byte) (abl >> 8);
            byte abn = (byte) (abl >> 16);
            uint abo = _0x595c14[abm];
            uint abp = _0x595814[abn];
            uint abq = abl >> 0x18;
            uint abr = abp ^ abo;
            uint abs = _0x595414[abq];
            uint abt = abl & 0x000000ff;
            uint abu = abr ^ abs;
            uint abv = _0x596014[abt];
            uint abw = abu ^ abv;
            byte abx = (byte) (abw >> 16);
            uint aby = abw >> 0x18;
            uint abz = _0x594C14[aby];
            uint aca = abz << 0x8;
            uint acb = _0x595014[abx];
            uint acc = aca ^ acb;
            byte acd = (byte) (abw >> 8);
            uint ace = abw & 0x000000ff;
            uint acf = acc << 0x8;
            uint acg = acf ^ _0x594C14[acd];
            uint ach = _0x595014[ace];
            uint aci = acg << 0x8;
            uint acj = aci ^ ach;
            uint ack = abl & 0x0000001f;
            uint acl = abk ^ acj;
            uint acm = 0x20 - ack;
            uint acn = abi << (byte) acm;
            uint aco = abi >> (byte) ack;
            uint acp = aae - acj;
            uint acq = acn | aco;
            byte acr = (byte) (acq >> 8);
            byte acs = (byte) (acq >> 16);
            uint act = _0x595814[acs];
            uint acu = _0x595c14[acr];
            uint acv = act ^ acu;
            uint acw = acq >> 0x18;
            uint acx = _0x595414[acw];
            uint acy = acv ^ acx;
            uint acz = acq & 0x000000ff;
            uint ada = _0x596014[acz];
            uint adb = acy ^ ada;
            byte adc = (byte) (adb >> 16);
            uint add = adb >> 0x18;
            uint ade = _0x594C14[add];
            uint adf = _0x595014[adc];
            uint adg = ade << 0x8;
            uint adh = adf ^ adg;
            uint adi = (byte) (adb >> 8);
            uint adj = adb & 0x000000ff;
            uint adk = adh << 0x8;
            uint adl = _0x594C14[adi];
            uint adm = adk ^ adl;
            uint adn = _0x595014[adj];
            uint ado = adm << 0x8;
            uint adp = ado ^ adn;
            uint adq = acq & 0x0000001f;
            uint adr = 0x20 - adq;
            uint ads = acl << (byte) adr;
            uint adt = acl >> (byte) adq;
            uint adu = acp ^ adp;
            uint adv = abl - adp;
            uint adw = ads | adt;
            byte adx = (byte) (adw >> 8);
            byte ady = (byte) (adw >> 16);
            uint adz = _0x595c14[adx];
            uint aea = _0x595814[ady];
            uint aeb = adz ^ aea;
            uint aec = adw >> 0x18;
            uint aed = adw & 0x000000ff;
            uint aee = _0x595414[aec];
            uint aef = _0x596014[aed];
            uint aeg = aeb ^ aee;
            uint aeh = aeg ^ aef;
            byte aei = (byte) (aeh >> 16);
            uint aej = aeh >> 0x18;
            uint aek = _0x595014[aei];
            uint ael = _0x594C14[aej];
            uint aem = ael << 0x8;
            uint aen = aek ^ aem;
            byte aeo = (byte) (aeh >> 8);
            uint aep = aeh & 0x000000ff;
            uint aeq = aen << 0x8;
            uint aer = _0x594C14[aeo];
            uint aes = keyState.Hash[68];
            uint aet = aeq ^ aer;
            uint aeu = _0x595014[aep];
            uint aev = aet << 0x8;
            uint aew = aev ^ aeu;
            uint aex = adw - aes;
            uint aey = acq - aew;
            uint aez = aey ^ keyState.Hash[69];
            uint afa = keyState.Hash[70];
            uint afb = aew ^ adv;
            uint afc = afb - afa;
            uint afd = adw & 0x0000001f;
            uint afe = 0x20 - afd;
            uint aff = adu << (byte) afe;
            uint afg = keyState.Hash[71];
            uint afh = adu >> (byte) afd;
            uint afi = aff | afh;
            uint afj = afi ^ afg;
            keyState.Output[0] = aex;
            keyState.Output[1] = aez;
            keyState.Output[2] = afc;
            keyState.Output[3] = afj;
        }
    }
}