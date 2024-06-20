import React, { useState } from 'react';

function UpdateWord() {
    const [inputValue, setInputValue] = useState({
        name: "xxxx",
        dict: [{
            url: "gg",
            count: 0,
        }
        ,{
            url: "hh",
            count: 3,
        }
        ,{
            url: "uu",
            count: 44,
        }
        ,{
            url: "iooiu",
            count: 55,
        }
        ,{
            url: "ee",
            count: 7,
        }
    ],
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setInputValue(prevState => ({
            ...prevState,
            [name]: value,
        }));
    };

  

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        console.log(JSON.stringify(inputValue));
        try {
            // Assuming inputValue.id is the dynamic ID
            const response = await fetch("http://localhost:5015/Words", {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    // Add other headers as needed
                },
                body: JSON.stringify(inputValue),
            });
    
            if (response.ok) {
                console.log('Data submitted successfully');
                // Handle successful response
            } else {
                console.error('Submission failed');
                // Handle errors
            }
        } catch (error) {
            console.error('There was an error submitting the data', error);
        }
    };
    



    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:
                <input type="text" name="name" value={inputValue.name} onChange={handleInputChange} />
            </label>
            <label>
                URL:
                <input type="text" name="url" value={inputValue.dict[4].url} onChange={handleInputChange} />
            </label>
            <label>
                Count:
                <input type="number" name="count" value={inputValue.dict[4].count} onChange={handleInputChange} />
            </label>
            <button type="submit">Submit</button>
        </form>
    );
}

export default UpdateWord;
