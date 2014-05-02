/*
* Kendo UI Localization Project for v2012.3.1114 
* Copyright 2012 Telerik AD. All rights reserved.
* 
* Simplified Chinese (zh-CN) Language Pack
*
* Project home  : https://github.com/loudenvier/kendo-global
* Kendo UI home : http://kendoui.com
* Author        : IKKI Phoenix  
*                 
*
* This project is released to the public domain, although one must abide to the 
* licensing terms set forth by Telerik to use Kendo UI, as shown bellow.
*
* Telerik's original licensing terms:
* -----------------------------------
* Kendo UI Web commercial licenses may be obtained at
* https://www.kendoui.com/purchase/license-agreement/kendo-ui-web-commercial.aspx
* If you do not own a commercial license, this file shall be governed by the
* GNU General Public License (GPL) version 3.
* For GPL requirements, please review: http://www.gnu.org/copyleft/gpl.html
*/

kendo.culture("zh-CN"); // Add by IKKI
kendo.ui.Locale = "Simplified Chinese (zh-CN)";

kendo.ui.ColumnMenu.prototype.options.messages =
	$.extend(kendo.ui.ColumnMenu.prototype.options.messages, {

	    /* COLUMN MENU MESSAGES 
         ****************************************************************************/
	    sortAscending: "昇冪排列",
	    sortDescending: "降冪排列",
	    filter: "篩選",
	    columns: "欄位列"
	    /***************************************************************************/
	});

kendo.ui.Groupable.prototype.options.messages =
	$.extend(kendo.ui.Groupable.prototype.options.messages, {

	    /* GRID GROUP PANEL MESSAGES 
         ****************************************************************************/
	    empty: "將欄位列名稱拖拽到此處可進行該列的分組顯示"
	    /***************************************************************************/
	});

kendo.ui.FilterMenu.prototype.options.messages =
	$.extend(kendo.ui.FilterMenu.prototype.options.messages, {

	    /* FILTER MENU MESSAGES 
         ***************************************************************************/
	    info: "篩選條件：",	// sets the text on top of the filter menu
	    filter: "篩選",		// sets the text for the "Filter" button
	    clear: "清空",		// sets the text for the "Clear" button
	    // when filtering boolean numbers
	    isTrue: "是",		// sets the text for "isTrue" radio button
	    isFalse: "否",		// sets the text for "isFalse" radio button
	    //changes the text of the "And" and "Or" of the filter menu
	    and: "並且",
	    or: "或者",
	    selectValue: "-= 請選擇 =-"
	    /***************************************************************************/
	});

kendo.ui.FilterMenu.prototype.options.operators =
	$.extend(kendo.ui.FilterMenu.prototype.options.operators, {

	    /* FILTER MENU OPERATORS (for each supported data type) 
         ****************************************************************************/
	    string: {
	        eq: "等於",
	        neq: "不等於",
	        contains: "包含",
	        doesnotcontain: "不包含",
	        startswith: "開始於",
	        endswith: "結束於"
	    },
	    number: {
	        eq: "等於",
	        neq: "不等於",
	        gt: "大於",
	        gte: "大於等於",
	        lt: "小於",
	        lte: "小於等於"
	    },
	    date: {
	        eq: "等於",
	        neq: "不等於",
	        gt: "晚於",
	        gte: "晚於等於",
	        lt: "早於",
	        lte: "早於等於"
	    },
	    enums: {
	        eq: "等於",
	        neq: "不等於"
	    }
	    /***************************************************************************/
	});

kendo.ui.Pager.prototype.options.messages =
	$.extend(kendo.ui.Pager.prototype.options.messages, {

	    /* PAGER MESSAGES 
         ****************************************************************************/
	    display: "{0} - {1} 筆　共 {2} 筆資料",
	    empty: "無資料",
	    page: "轉到第",
	    of: "頁　共 {0} 頁",
	    itemsPerPage: "筆每頁",
	    first: "首頁",
	    previous: "上一頁",
	    next: "下一頁",
	    last: "尾頁",
	    refresh: "刷新"
	    /***************************************************************************/
	});

kendo.ui.Validator.prototype.options.messages =
	$.extend(kendo.ui.Validator.prototype.options.messages, {

	    /* VALIDATOR MESSAGES 
         ****************************************************************************/
	    required: "{0} 是必填項！",
	    pattern: "{0} 的格式不正確！",
	    min: "{0} 必須大於或等於 {1} ！",
	    max: "{0} 必須小於或等於 {1} ！",
	    step: "{0} 不是正確的步進值！",
	    email: "{0} 不是正確的電子郵件！",
	    url: "{0} 不是正確的網址！",
	    date: "{0} 不是正確的日期！"
	    /***************************************************************************/
	});

// The upload part add by IKKI
kendo.ui.Upload.prototype.options.localization =
	$.extend(kendo.ui.Upload.prototype.options.localization, {

	    /* UPLOAD LOCALIZATION
         ****************************************************************************/
	    select: "選擇檔",
	    dropFilesHere: "將文件拖拽到此處上傳",
	    cancel: "取消",
	    remove: "移除",
	    uploadSelectedFiles: "上傳文件",
	    statusUploading: "上傳中……",
	    statusUploaded: "上傳成功！",
	    statusFailed: "上傳失敗！",
	    retry: "重試"
	    /***************************************************************************/
	});

kendo.ui.ImageBrowser.prototype.options.messages =
	$.extend(kendo.ui.ImageBrowser.prototype.options.messages, {

	    /* IMAGE BROWSER MESSAGES 
         ****************************************************************************/
	    uploadFile: "上傳文件",
	    orderBy: "排序方式",
	    orderByName: "按名稱排序",
	    orderBySize: "按大小排序",
	    directoryNotFound: "目錄未找到",
	    emptyFolder: "空資料夾",
	    deleteFile: '你確定要刪除【{0}】這個檔嗎？',
	    invalidFileType: "你上傳的檔案格式 {0} 是無效的，支持的檔案類型為：{1}",
	    overwriteFile: "一個名為【{0}】的檔已經存在，是否覆蓋？",
	    dropFilesHere: "將文件拖拽到此處上傳"
	    /***************************************************************************/
	});

kendo.ui.Editor.prototype.options.messages =
	$.extend(kendo.ui.Editor.prototype.options.messages, {

	    /* EDITOR MESSAGES 
         ****************************************************************************/
	    bold: "粗體",
	    italic: "斜體",
	    underline: "底線",
	    strikethrough: "刪除線",
	    superscript: "上標",
	    subscript: "下標",
	    justifyCenter: "居中對齊",
	    justifyLeft: "左對齊",
	    justifyRight: "右對齊",
	    justifyFull: "兩端對齊",
	    insertUnorderedList: "插入無序列表",
	    insertOrderedList: "插入有序列表",
	    indent: "增加縮進",
	    outdent: "減少縮進",
	    createLink: "插入連結",
	    unlink: "刪除連結",
	    insertImage: "插入圖片",
	    insertHtml: "插入HTML",
	    fontName: "請選擇字體",
	    fontNameInherit: "（預設字體）",
	    fontSize: "請選擇字型大小",
	    fontSizeInherit: "（默認字型大小）",
	    formatBlock: "格式",
	    foreColor: "文字顏色",
	    backColor: "文字背景色",
	    style: "樣式",
	    emptyFolder: "空資料夾",
	    uploadFile: "上傳文件",
	    orderBy: "排序方式：",
	    orderBySize: "按大小排序",
	    orderByName: "按名稱排序",
	    invalidFileType: "你上傳的檔案格式 {0} 是無效的，支持的檔案類型為：{1}",
	    deleteFile: '你確定要刪除【{0}】這個檔嗎？',
	    overwriteFile: '一個名為【{0}】的檔已經存在，是否覆蓋？',
	    directoryNotFound: "目錄未找到",
	    imageWebAddress: "圖片連結位址",
	    imageAltText: "圖片預留位置",
	    linkWebAddress: "連結位址",
	    linkText: "連結文字",
	    linkToolTip: "文字提示",
	    linkOpenInNewWindow: "是否在新視窗中打開",
	    dialogInsert: "插入",
	    dialogButtonSeparator: "或",
	    dialogCancel: "取消"
	    /***************************************************************************/
	});

