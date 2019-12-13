<?php
$phones = simplexml_load_file("book.xml");
echo $phones -> contact[0] -> attributes() -> id;
$counter = 0;
//редактирование
$filexml = new DOMDocument;
$filexml = simplexml_load_file("book.xml");


function searchPhonesBySurname($query){
    global $phones;
    $result = array();
    foreach ($phones -> contact as $computer){
        if (substr(strtolower($computer -> surname), 0, strlen($query))==strtolower($query))
            array_push($result, $computer);
    }
    return $result;
}

?>
<!DOCTYPE html>
<html>
    <head>
        <title>Phone Contacts</title>		
        <meta charset="utf-8">
      <link rel="stylesheet" type="text/css" href="design.css">
    </head>
    <body>			
		<nav>
		  <ul>
			<li onclick='window.location="Index.php"'><a href="Index.php">Main page</a></li>
			<li onclick='window.location="SearchName.php"'><a href="SearchName.php">Search name</a></li>
			<li onclick='window.location="SearchSurname.php"'><a href="SearchSurname.php">Search surname</a></li>
			<li onclick='window.location="SearchPhone.php"'><a href="SearchPhone.php">Search phone</a></li>
			<li onclick='window.location="SearchEmail.php"'><a href="SearchEmail.php">Search email</a></li>
			<li onclick='window.location="SearchGender.php"'><a href="SearchGender.php">Search gender</a></li>
		  </ul>  
		</nav>
		<h3><p align="center">Phone book contacts<p></h3> 	
		<!--Показывает первый и последний номер телефона в таблице-->
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<b>First phone:</b>
        <b class="lead">
        <?php
        echo $phones -> contact[0] -> phone;
        ?>
        </b>
        <br/>
        <b>Last phone:</b>
        <b class="lead">

            <?php
            $var_phone=$phones->xpath('/book/contact[last()]/phone')[0];
            echo $var_phone;
            ?>
        </b>
		
        <table class="features-table">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Surname</th>
                <th>Phone</th>
                <th>Email</th>
				<th>Gender</th>
            </tr>
        </thead>
            <?php
          
            foreach($phones -> contact as $arvuti) {
                echo "<tr>";
                echo "<td>".($arvuti -> id)."</td>";
                echo "<td>".($arvuti -> name)."</td>";
                echo "<td>".($arvuti -> surname)."</td>";
                echo "<td>".($arvuti -> phone)."</td>";
                echo "<td>".($arvuti -> email)."</td>";
                echo "<td>".($arvuti -> gender)."</td>";
                echo "</tr>";
                }
            
            ?>
        </table>

      
    

    <!--Поиск по фамилии-->
         <br />
         <br />
        <form action="?" method="post">
            <h3>Search surname:</h3> <input type="text" name="searchSurname" placeholder="Surname"/>
            <input type="submit" value="Find" />
        </form>
        <table class="features-table">
            <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Surname</th>
                <th>Phone</th>
                <th>Email</th>
				<th>Gender</th>
                
            </tr>
            </thead>
            <?php
            if(!empty($_POST["searchSurname"])){
            $result = searchPhonesBySurname($_POST["searchSurname"]);
            foreach($result as $arvuti) {
                $counter++;
                echo "<tr>";
                echo "<td>".($arvuti -> id)."</td>";
                echo "<td>".($arvuti -> name)."</td>";
                echo "<td><ins>".($arvuti -> surname)."</ins></td>";
                echo "<td>".($arvuti -> phone)."</td>";
                echo "<td>".($arvuti -> email)."</td>";
				echo "<td>".($arvuti -> gender)."</td>";
                echo "</tr>";
                }
                echo "<h4>".($counter)." result found</h4>";
            }
            ?>


        </table>
    </body>
</html>