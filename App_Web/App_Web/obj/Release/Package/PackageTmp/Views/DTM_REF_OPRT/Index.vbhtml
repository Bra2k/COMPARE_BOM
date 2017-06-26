@ModelType IEnumerable(Of App_Web.DTM_REF_OPRT)

@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <p>
        @Html.ActionLink("Create New", "Create")
    </p>
    <table class="table">
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.LB_INIT)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.ID_HBLT_ALMS)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.BL_ATVT)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.DT_ATVT)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.ID_MTCL_ECO)
            </th>
            <th></th>
        </tr>
    
    @For Each item In Model
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) item.LB_INIT)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ID_HBLT_ALMS)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.BL_ATVT)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.DT_ATVT)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ID_MTCL_ECO)
            </td>
            <td>
                @Html.ActionLink("Edit", "Edit", New With {.id = item.ID_OPRT }) |
                @Html.ActionLink("Details", "Details", New With {.id = item.ID_OPRT }) |
                @Html.ActionLink("Delete", "Delete", New With {.id = item.ID_OPRT })
            </td>
        </tr>
    Next
    
    </table>
</body>
</html>
