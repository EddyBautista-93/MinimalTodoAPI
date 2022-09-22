import React, { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [isLoading, setIsLoading] = useState(true);
  const [data, setData] = useState([]); // Save Api response with this. 
  
  useEffect(() => {
    var raw = "";
    var requestOptions = {
      method: 'GET',
      redirect: 'follow'
    };


    const url = "http://localhost:5103/todoitems";
    const fetchData = async () => {
      try {
        const response = await fetch(url);
        const json = await response.json();
        setData(json);
        console.log(json);
        console.log(json[0].name)
      } catch (error) {
        console.log("error;",error)
      }
    };
    fetchData()  
  }, [])

  useEffect(() => {
    if (data.length !== 0) {
      setIsLoading(false);
    }
    console.log(data);
  }, [data]);
  

  return (
    <div>
      {isLoading ? (
        <h1>Loading...</h1>
      ) : (
        data.map((json) => (
          <h1>
            {json.name}
          </h1>
        ))
      )}
    </div>
  );
}

export default App
