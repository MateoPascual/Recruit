import React from 'react';
import { Chart } from 'primereact/components/chart/Chart';
import { activityCalc } from './server';

//WHEN USING, PASS IN ID
//        <ActivityCalcF
//          Id={} <= THE USER ID VALUE HERE
//        />

class ActivityCalc extends React.Component
{
    state = {
        xAxis:[],
        yAxis:[],
    }

    componentDidMount()
    {
        let data = {};
        data.Id = this.props.id;
        const myPromise = activityCalc(data);
            myPromise.then(resp =>{
            this.parseArray(resp.data.items);
        });
    }

    parseArray(data)
    {
        if(!data.length)
        {
            return
        }
        let xAxis1 = [];
        let yAxis1 = [];
        data.forEach( element => {
            xAxis1.push(element.monthAndDate);
            yAxis1.push(element.numberOfPosts);
        });
        this.setState({
            xAxis : xAxis1,
            yAxis : yAxis1
        });
    }

    render()
    {
        var data = {
            labels: this.state.xAxis,
            datasets: [
                {
                    label: 'Videos Posted',
                    data: this.state.yAxis,
                    fill: true,
                    innerColor: 'blue',
                    borderColor: 'maroon'
                },
            ]   
        };
        return(
            <React.Fragment>
                {(this.state.xAxis.length===0)?
                    <div>
                        <h1>User Has Yet To Post</h1>
                    </div>
                : 
                    <div className="content-section implementation">
                        <Chart type="bar" data={data} />
                    </div> 
                }              
            </React.Fragment>
        );
    }
}

export default ActivityCalc;