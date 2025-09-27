/* set global variable i */

var i=1; 

function increment(){
i +=1;                         /* function for automatic increament of feild's attribute*/                 
}

/* 
---------------------------------------------

function to remove fom elements dynamically
---------------------------------------------

*/

function removeElement(parentDiv, childDiv){
 

     if (childDiv == parentDiv) {
          alert("The parent div cannot be removed.");
     }
     else if (document.getElementById(childDiv)) {     
          var child = document.getElementById(childDiv);
          var parent = document.getElementById(parentDiv);
          parent.removeChild(child);
     }
     else {
          alert("Child div has already been removed or does not exist.");
          return false;
     }
}



/* 
 ----------------------------------------------------------------------------
 
 functions that will be called upon, when user click on add field
 
 ---------------------------------------------------------------------------
 */
function add_fields()
{
var div1=document.createElement('div');
div1.setAttribute("class","form-group");
var div2=document.createElement("div");
div2.setAttribute("class","col-md-3");
var span1=document.createElement("span");
span1.setAttribute("class", "help-block-2");
var input1 = document.createElement("input");
input1.setAttribute("type", "text");
input1.setAttribute("class", "form-control");

var div3=document.createElement("div");
div3.setAttribute("class","col-md-2");
var span2=document.createElement("span");
span2.setAttribute("class", "help-block-2");
var text2 = document.createTextNode("Từ ngày:");
var input2 = document.createElement("input");
input2.setAttribute("type", "text");
input2.setAttribute("class", "form-control");

var div4=document.createElement("div");
div4.setAttribute("class","col-md-2");

var div5=document.createElement("div");
div5.setAttribute("class","col-md-3");
var span3=document.createElement("span");
span3.setAttribute("class", "help-block-2");
var text3 = document.createTextNode("Giáo vụ lệnh:");
var input3 = document.createElement("input");
input3.setAttribute("type", "file");
input3.setAttribute("class", "form-control");

//var div6=document.createElement("div");
//div6.setAttribute("class","col-md-2");
//var img=document.createElement("img");
//img.setAttribute("src","img/delete.png");
//img.setAttribute("style","cursor: pointer;");

increment();
var text1 = document.createTextNode("Nhiệm sở thứ "+i);
input1.setAttribute("name", "");
input2.setAttribute("name", "");
input3.setAttribute("name", "");
//img.setAttribute("onclick", "removeElement('add_wrapper','id_"+ i +"')");
span1.appendChild(text1);
span2.appendChild(text2);
span3.appendChild(text3);
div2.appendChild(span1);
div2.appendChild(input1);
div3.appendChild(span2);
div3.appendChild(input2);
div5.appendChild(span3);
div5.appendChild(input3);
div6.appendChild(img);
div1.appendChild(div2);
div1.appendChild(div3);
div1.appendChild(div4);
div1.appendChild(div5);
div1.appendChild(div6);
div1.setAttribute("id", "id_"+i);
document.getElementById("add_wrapper").appendChild(div1);
}


/* 
 ----------------------------------------------------------------------------
 
 functions that will be called upon, when user click on field
 
 ---------------------------------------------------------------------------
 */
function addChild()
{
var r=document.createElement('span');
var y = document.createElement("INPUT");
y.setAttribute("type", "text");
y.setAttribute("placeholder","Name");
var g = document.createElement("IMG");
g.setAttribute("src", "delete.png");
increment(); 
y.setAttribute("Name","textelement_"+i);
r.appendChild(y);
g.setAttribute("onclick", "removeElement('myForm','id_"+ i +"')");
r.appendChild(g);
r.setAttribute("id", "id_"+i);
document.getElementById("myForm").appendChild(r);
}




 /* 
 ----------------------------------------------------------------------------
 
 functions that will be called upon, when user click on the Name text field
 
 ---------------------------------------------------------------------------
 */
function nameFunction()
{
var r=document.createElement('span');
var y = document.createElement("INPUT");
y.setAttribute("type", "text");
y.setAttribute("placeholder","Name");
var g = document.createElement("IMG");
g.setAttribute("src", "delete.png");
increment(); 
y.setAttribute("Name","textelement_"+i);
r.appendChild(y);
g.setAttribute("onclick", "removeElement('myForm','id_"+ i +"')");
r.appendChild(g);
r.setAttribute("id", "id_"+i);
document.getElementById("myForm").appendChild(r);
}


/*
-----------------------------------------------------------------------------

functions  that will be called upon, when user click on the Email text field

------------------------------------------------------------------------------
*/
function emailFunction()
{
var r=document.createElement('span');
var y = document.createElement("INPUT");
y.setAttribute("type", "text");
y.setAttribute("placeholder", "Email");
var g = document.createElement("IMG");
g.setAttribute("src", "delete.png");
increment();
y.setAttribute("Name","textelement_"+i);
r.appendChild(y);
g.setAttribute("onclick", "removeElement('myForm','id_"+ i +"')");
r.appendChild(g);
r.setAttribute("id", "id_"+i);
document.getElementById("myForm").appendChild(r);
}

/*
-----------------------------------------------------------------------------

functions  that will be called upon, when user click on the Contact text field

------------------------------------------------------------------------------
*/

function contactFunction()
{
var r=document.createElement('span');
var y = document.createElement("INPUT");
y.setAttribute("type", "text");
y.setAttribute("placeholder", "Contact");
var g = document.createElement("IMG");
g.setAttribute("src", "delete.png");
increment();
y.setAttribute("Name","textelement_"+i);
r.appendChild(y);
g.setAttribute("onclick", "removeElement('myForm','id_"+ i +"')");
r.appendChild(g);
r.setAttribute("id", "id_"+i);
document.getElementById("myForm").appendChild(r);
}

/*
-----------------------------------------------------------------------------

functions  that will be called upon, when user click on the Messege textarea field

------------------------------------------------------------------------------
*/

function textareaFunction()
{
var r=document.createElement('span');

var y = document.createElement("TEXTAREA");
var g = document.createElement("IMG");
y.setAttribute("cols", "17");
y.setAttribute("placeholder", "message..");
g.setAttribute("src", "delete.png");
increment();
y.setAttribute("Name","textelement_"+i);
r.appendChild(y);
g.setAttribute("onclick", "removeElement('myForm','id_"+ i +"')");
r.appendChild(g);
r.setAttribute("id", "id_"+i);
document.getElementById("myForm").appendChild(r);

}

/*
-----------------------------------------------------------------------------

functions  that will be called upon, when user click on the Reset Button

------------------------------------------------------------------------------
*/
function resetElements(){
document.getElementById('myForm').innerHTML = '';
}

