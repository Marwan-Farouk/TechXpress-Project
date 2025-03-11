@using PresentationLayer.ActionRequests.Products
@model CreateProductActionRequest

<form action="/Product/Create" method="post" enctype="multipart/form-data">
    <div class= "form-group" >
        < label for= "name" > Name :</ label >
        < input type = "text" class= "form-control" id = "name" name = "name" >
    </ div >
    < div class= "form-group" >
        < label for= "price" > Price :</ label >
        < input type = "text" class= "form-control" id = "price" name = "price" >
    </ div >
    < div class= "form-group" >
        < label for= "description" > Description :</ label >
        < input type = "text" class= "form-control" id = "description" name = "description" >
    </ div >
    < div class= "form-group" >
        < label for= "image" > Image :</ label >
        < input type = "file" class= "form-control-file" id = "image" name = "image" >
    </ div >
    < button type = "submit" class= "btn btn-success" > Create Product </ button >
</ form >