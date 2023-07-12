import React from 'react'
import { Pie } from 'react-chartjs-2'

class DataCard extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
        };
    }

    render(){
       
        return(                   
                <Pie data={{
                    labels: this.props.labels,    
                    datasets: [
                        {
                        label: 'Dataset 1',
                        data: this.props.data,
                        backgroundColor: [
                            '#26756B', '#99F7EA', '#4EF4DF', '#49756F', '#3EC2B0'
                        ]
                        }
                    ]                  
                }}              
                height={50}
                widht={50}
                options={{
                    indexAxis: 'y',
                    // Elements options apply to all of the options unless overridden in a dataset
                    // In this case, we are setting the border of each horizontal bar to be 2px wide
                    elements: {
                    bar: {
                        borderWidth: 2,
                    }
                    },
                    responsive: true,
                    plugins: {
                    legend: {
                        position: 'right',
                    },
                    title: {
                        display: true,
                        text: 'Top 5 HashTags'
                    }
                    }
                }}
            />            
        ) 
    }
}
  
  export default DataCard