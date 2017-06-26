@ModelType App_Web.DTM_REF_OPRT

@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Details</title>
</head>
<body>
    <div>
        <h4>DTM_REF_OPRT</h4>
        <hr />
        <dl class="dl-horizontal">
            <dt>
                @Html.DisplayNameFor(Function(model) model.LB_INIT)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.LB_INIT)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.ID_HBLT_ALMS)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.ID_HBLT_ALMS)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.BL_ATVT)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.BL_ATVT)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.DT_ATVT)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.DT_ATVT)
            </dd>
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.ID_MTCL_ECO)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.ID_MTCL_ECO)
            </dd>
    
        </dl>
    </div>
    <p>
        @Html.ActionLink("Edit", "Edit", New With { .id = Model.ID_OPRT }) |
        @Html.ActionLink("Back to List", "Index")
    </p>
</body>
</html>
