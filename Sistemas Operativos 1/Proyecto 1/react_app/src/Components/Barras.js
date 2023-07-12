import React from 'react'
import { Bar } from 'react-chartjs-2'

class DataCard extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
        };
    }

    render(){
       
        return(                   
                <Bar data={{
                    labels: this.props.labels,        
                    datasets: [{
                        label: 'UpVotes',
                        data: this.props.upvotes,
                        borderColor: '#26756B',
                        backgroundColor: '#4EF4DF',
                        borderWidth: 2,
                        borderRadius: 40,
                        borderSkipped: false,
                    },
                    {
                        label: 'DownVotes',
                        data: this.props.downvotes,
                        borderColor: '#4EF4DF',
                        backgroundColor: '#26756B',
                        borderWidth: 2,
                        borderRadius: 40,
                        borderSkipped: false,
                    }]                    
                }}              
                height={this.props.altura}
                widht={100}
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
                        text: 'Up And Down Votes By Day'
                    }
                    }
                }}
            />            
        ) 
    }
}
  
  export default DataCard