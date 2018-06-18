import React from "react";
import EventsCalender from "./components/EventsCalender";
//  Components

class CalendarTab extends React.Component {
  render() {
    return (
      <React.Fragment>
        <div className="mt-10">
          <EventsCalender />
        </div>
      </React.Fragment>
    );
  }
}

export default CalendarTab;
