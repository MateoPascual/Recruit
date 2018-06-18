import React, { Component, Router } from "react";
import BigCalendar from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import {
  userEventsGet,
  events_create,
  events_update,
  events_delete,
  GetPublicEvents
} from "../../server";
import { ValidationMessage } from "../../Validation";
import { displayComponent } from "../../utils";
import { showModal } from "../../SmallModal";

BigCalendar.setLocalizer(BigCalendar.momentLocalizer(moment));

const allViews = Object.keys(BigCalendar.Views).map(k => BigCalendar.Views[k]);

class EventsCalender extends React.Component {
  name = React.createRef();
  link = React.createRef();
  createdBy = React.createRef();
  modifiedBy = React.createRef();
  startDate = React.createRef();
  endDate = React.createRef();
  logo = React.createRef();
  description = React.createRef();
  organizer = React.createRef();

  state = {
    view: "day",
    date: new Date(),
    width: 500,
    events1: [],
    events2: [],
    holder: "https://www.knightslimo.com/images/site/baseball-field.jpg",
    id: null,
    name: "",
    startDate: "",
    endDate: "",
    description: "",
    link: "",
    logo: "",
    isOngoing: false,
    organizer: "",
    createdBy: "",
    modifiedBy: "",
    btnName: "Submit",
    events: [],
    trigger: false,
    disabled: false,
    private: "",
    formTitle: "Create or Edit Event"
  };

  componentDidMount() {
    const myPromise = userEventsGet();
    myPromise.then(resp => {
      console.log(resp);
      this.sendToEvents(resp.data.items, 1);
      this.secondPromise();
    });
  }

  secondPromise = () => {
    const myPromise2 = GetPublicEvents();
    myPromise2.then(resp => {
      console.log(resp);
      this.sendToEvents(resp.data.items, 0);
    });
  };
  sendToEvents = (data, x) => {
    let array = data;
    let arrayToPush = this.state.events;
    let createByChecker = "";
    let modifiedByChecker = "";
    for (let i = 0; i < array.length; i++) {
      let objToPush = {};
      objToPush.title = array[i].name;
      objToPush.start = new Date(
        Date.parse(
          moment
            .utc(array[i].startDate)
            .local()
            .format("YYYY-MM-DDTHH:mm")
        )
      );
      objToPush.end = new Date(
        Date.parse(
          moment
            .utc(array[i].endDate)
            .local()
            .format("YYYY-MM-DDTHH:mm")
        )
      );
      objToPush.desc = array[i].description;
      objToPush.link = array[i].link;
      objToPush.link = array[i].link;
      objToPush.logo = array[i].logo;
      objToPush.isOngoing = array[i].isOngoing;
      objToPush.private = array[i].private;
      objToPush.organizer = array[i].organizer;
      objToPush.id = array[i].id;
      if (x === 1) {
        objToPush.style = true;
        createByChecker = array[i].createdBy;
        modifiedByChecker = array[i].modifiedBy;
        console.log("this");
      } else {
        objToPush.style = false;
        if (this.state.createdBy === array[i].createdBy) {
          console.log("THIS");
          continue;
        }
      }
      arrayToPush.push(objToPush);
      //console.log(JSON.stringify(array[i], null, 3) , 'this is logged data');
    }
    if (x === 1) {
      this.setState({ createdBy: createByChecker });
      console.log(this.state.createdBy);
      this.setState({ modifiedBy: modifiedByChecker });
    }
    this.setState({ events: arrayToPush });
  };

  onChangeStartDate = e => {
    this.setState({ startDate: e.target.value });
  };

  onChangeEndDate = e => {
    this.setState({ endDate: e.target.value });
  };

  onChangeDescription = e => {
    this.setState({ description: e.target.value });
  };

  onChangeLink = e => {
    this.setState({ link: e.target.value });
  };

  onChangeLogo = e => {
    this.setState({ logo: e.target.value });
  };

  onChangeIsOngoing = e => {
    this.setState({ private: e.target.value });
    this.setState({ private: e.target.checked });
  };

  onChangeOrganizer = e => {
    this.setState({ organizer: e.target.value });
  };

  onChangeName = e => {
    this.setState({ name: e.target.value });
  };

  onChangeDisabled = event => {
    this.setState({ disabled: true });
    this.onEventClick(event);
  };

  onChangeEnabled = event => {
    this.setState({ disabled: false });
    this.onEventClick(event);
  };

  onEventClick = event => {
    this.setState({
      endDate: moment
        .utc(event.end)
        .local()
        .format("YYYY-MM-DDTHH:mm")
    });
    this.setState({
      startDate: moment
        .utc(event.start)
        .local()
        .format("YYYY-MM-DDTHH:mm")
    });
    this.setState({ description: event.desc });
    this.setState({ name: event.title });
    this.setState({ link: event.link });
    this.setState({ logo: event.logo });
    this.setState({ Ongoing: event.Ongoing });
    this.setState({ organizer: event.organizer });
    this.setState({ id: event.id });
    this.setState({ private: event.private });
    this.setState({ btnName: "Edit" });
    this.setState({ trigger: true });
  };

  handlerSubmit = e => {
    e.preventDefault();
    const This = this;
    if (this.state.btnName === "Submit") {
      events_create({
        name: this.state.name,
        startDate: this.state.startDate
          ? moment(this.state.startDate)
              .utc()
              .format("YYYY-MM-DD HH:mm")
          : null,
        endDate: this.state.endDate
          ? moment(this.state.endDate)
              .utc()
              .format("YYYY-MM-DD HH:mm")
          : null,
        description: this.state.description,
        link: this.state.link,
        logo: this.state.logo,
        isOngoing: this.state.isOngoing,
        organizer: this.state.organizer,
        createdBy: this.state.createdBy,
        modifiedBy: this.state.modifiedBy,
        private: this.state.private
      })
        .then(data => {
          showModal({
            title: "Submission Confirmation",
            body: this.state.name + " submitted successfully."
          });
          window.location.reload();
        })
        .catch(error => {
          This.setState({ disabled: "" });
          alert(
            "An error has occurred while submitting your information to the server.  Please try again later."
          );
        });
    } else {
      events_update({
        id: this.state.id,
        name: this.state.name,
        startDate: this.state.startDate
          ? moment(this.state.startDate)
              .utc()
              .format("YYYY-MM-DD HH:mm")
          : null,
        endDate: this.state.endDate
          ? moment(this.state.endDate)
              .utc()
              .format("YYYY-MM-DD HH:mm")
          : null,
        description: this.state.description,
        link: this.state.link,
        logo: this.state.logo,
        isOngoing: this.state.isOngoing,
        organizer: this.state.organizer,
        createdBy: this.state.createdBy,
        modifiedBy: this.state.modifiedBy,
        private: this.state.private
      })
        .then(data => {
          showModal({
            title: "Submission Confirmation",
            body: this.state.name + " submitted successfully."
          });
          window.location.reload();
        })
        .catch(error => {
          This.setState({ disabled: "" });
          alert(
            "An error has occurred while submitting your information to the server.  Please try again later."
          );
        });
      return;
    }
  };

  deleteEvent = () => {
    let data = {};
    data.id = this.state.id;

    showModal({
      title: "Delete Confirmation",
      body: "You are about to delete the event "
    }).then(() => {
      events_delete(data)
        .then(data => {
          console.log(data.data);
          this.eventsGetAll();
          window.location.reload();
          return;
        })
        .catch(error =>
          alert(
            "An error has occurred while connecting to the server.  Please try again later."
          )
        );
    });
  };

  reloadPage = e => {
    e.preventDefault();
    window.location.reload();
  };

  render() {
    return (
      <div>
        {!this.state.events.length === 0 ? (
          <React.Fragment>
            <i class="fa fa-spinner fa-spin fa-2x" />
            <span> Loading...</span>
          </React.Fragment>
        ) : (
          <React.Fragment>
            <div style={{ height: 700 }}>
              <BigCalendar
                //style={{ height: 500, width: this.state.width }}

                toolbar={true}
                events={this.state.events}
                step={60}
                views={allViews}
                //view={this.state.view}
                onView={() => {}}
                date={this.state.date}
                onNavigate={date => this.setState({ date })}
                scrollToTime={new Date(1970, 1, 1, 6)}
                onSelectEvent={event =>
                  event.style
                    ? this.onChangeEnabled(event)
                    : this.onChangeDisabled(event)
                }
                onSelectSlot={slotInfo =>
                  alert(
                    `selected slot: \n\nstart ${slotInfo.start.toLocaleString()} ` +
                      `\nend: ${slotInfo.end.toLocaleString()}` +
                      `\naction: ${slotInfo.action}`
                  )
                }
                eventPropGetter={(event, start, end, isSelected) => {
                  let newStyle = {
                    backgroundColor: "navy",
                    color: "white",

                    borderRadius: "5px",
                    border: "none"
                  };

                  if (event.style) {
                    newStyle.backgroundColor = "maroon";
                  }

                  return {
                    className: "",
                    style: newStyle
                  };
                }}
              />
              <br />
              <div>
                {this.state.trigger === true ? (
                  <div className="panel panel-brand">
                    <div style={{ padding: ".5em" }}>
                      <div className="panel-heading">
                        <div className="pull-left">
                          <h3 className="panel-title">{this.state.name}</h3>
                        </div>
                        <div className="clearfix" />
                      </div>
                      <div className="col-md-offset-1">
                        <h4> {this.state.organizer} </h4>
                        <h6>
                          From:{" "}
                          {moment(this.state.startDate)
                            .utc()
                            .format("dddd, MMMM Do YYYY, h:mm:ss a")}{" "}
                          <br />
                          To:{" "}
                          {moment(this.state.endDate)
                            .utc()
                            .format("dddd, MMMM Do YYYY, h:mm:ss a")}{" "}
                          <br />
                        </h6>
                        <img
                          src={
                            this.state.logo.length === 0
                              ? this.state.holder
                              : this.state.logo
                          }
                        />
                        <blockquote> {this.state.description} </blockquote>
                        <a href={this.state.link}>link</a>
                      </div>
                    </div>
                  </div>
                ) : null}
              </div>
              <div className={this.state.formClass}>
                <div className="panel-heading">
                  <div className="pull-left">
                    <h3 className="panel-title">
                      <b>{this.state.formTitle}</b>
                    </h3>
                  </div>
                  <div className="clearfix" />
                </div>

                <div className="panel-body no-padding">
                  <form onSubmit={this.handlerSubmit}>
                    <fieldset disabled={this.state.disabled}>
                      <p className="text-muted">
                        Required <span className="asterisk">*</span>
                      </p>
                      <div className="form-body">
                        <div className="form-group">
                          <label className="control-label">
                            Name <span className="asterisk">*</span>:
                          </label>
                          <br />
                          <input
                            type="text"
                            ref={this.name}
                            className="form-control"
                            value={this.state.name}
                            maxLength="100"
                            required
                            placeholder="Ex: Joe's Fundraiser"
                            onChange={this.onChangeName}
                          />
                          <ValidationMessage
                            validationFor={this.name}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">
                            Start Date-Time:
                          </label>
                          <br />
                          <input
                            type={this.state.startDateType}
                            className="form-control"
                            onFocus={() =>
                              this.setState({
                                startDateType: "datetime-local"
                              })
                            }
                            placeholder="mm/dd/yyyy hh:mm --"
                            value={this.state.startDate}
                            ref={this.startDate}
                            onInput={this.onChangeStartDate}
                          />
                          <ValidationMessage
                            validationFor={this.startDate}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">
                            End Date-Time:
                          </label>
                          <br />
                          <input
                            type={this.state.endDateType}
                            className="form-control"
                            onFocus={() =>
                              this.setState({
                                endDateType: "datetime-local"
                              })
                            }
                            placeholder="mm/dd/yyyy hh:mm --"
                            value={this.state.endDate}
                            ref={this.endDate}
                            onChange={this.onChangeEndDate}
                          />
                          <ValidationMessage
                            validationFor={this.endDate}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">Description:</label>
                          <br />
                          <textarea
                            className="form-control"
                            value={this.state.description}
                            placeholder="Ex: To support Joe..."
                            maxLength="250"
                            ref={this.description}
                            onChange={this.onChangeDescription}
                          />
                          <ValidationMessage
                            validationFor={this.description}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">Link:</label>
                          <br />
                          <input
                            type="url"
                            className="form-control"
                            placeholder="Ex: http://www.example.com"
                            maxLength="100"
                            // minLength="20"
                            value={this.state.link}
                            onChange={this.onChangeLink}
                            ref={this.link}
                          />
                          <ValidationMessage
                            validationFor={this.link}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">Logo:</label>
                          <br />
                          <input
                            type="url"
                            className="form-control"
                            maxLength="100"
                            ref={this.logo}
                            value={this.state.logo}
                            placeholder="Ex: http://www.example.com/img.jpg"
                            onChange={this.onChangeLogo}
                          />
                          <ValidationMessage
                            validationFor={this.logo}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>

                        <div className="form-group">
                          <label className="control-label">
                            <input
                              type="checkbox"
                              className=""
                              checked={this.state.private}
                              value={this.state.private}
                              onChange={this.onChangeIsOngoing}
                            />{" "}
                            : Public
                          </label>
                        </div>

                        <div className="form-group">
                          <label className="control-label">Organizer:</label>
                          <br />
                          <input
                            type="text"
                            className="form-control"
                            value={this.state.organizer}
                            maxLength="250"
                            ref={this.organizer}
                            onChange={this.onChangeOrganizer}
                            placeholder="Ex: John Doe"
                          />
                          <ValidationMessage
                            validationFor={this.organizer}
                            validationTrigger={this.state.validationTrigger}
                          />
                        </div>
                      </div>
                    </fieldset>
                    <button
                      type="submit"
                      className="btn btn-success"
                      disabled={this.state.disabled}
                      onClick={() =>
                        this.setState({
                          validationTrigger: true
                        })
                      }
                    >
                      {this.state.btnName}
                    </button>
                    <span className="validText">
                      &nbsp;{this.state.successText}
                    </span>
                    {this.state.btnName === "Edit" ? (
                      <button
                        className="btn pull-right btn-danger"
                        onClick={this.deleteEvent}
                        disabled={this.state.disabled}
                      >
                        Delete{" "}
                      </button>
                    ) : null}
                    {this.state.btnName === "Edit" ? (
                      <button
                        className="btn pull-center btn-primary"
                        onClick={e => this.reloadPage(e)}
                      >
                        {" "}
                        Create New Event{" "}
                      </button>
                    ) : null}
                  </form>
                </div>
              </div>
            </div>
          </React.Fragment>
        )}
      </div>
    );
  }
}

export default EventsCalender;
