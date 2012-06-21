<%@ Control Language="C#" AutoEventWireup="true" %>
<% using(Html.BeginScript(ScriptPositionEnum.EndOfPage)) { %>
    $(document).ready(function () { 
        ko.applyBindings(window.viewModel);
    });
<% } %>
