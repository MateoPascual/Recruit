import React from "react";
import { user_followers, users_getById } from "./server";
import "./UserFollowers.css";
import UserFollowers from "./UserFollowers";

class UserFollowersTopFive extends React.Component {
  state = {
    data: [],
    flag: false,
    displayShortOrFull: true
  };

  componentDidMount() {
    const data = {};
    data.Id = this.props.id;
    data.GetShortList = true;
    let promises = [];
    const myPromise = user_followers(data);
    const newPromise = myPromise.then(resp => {
      this.setState({ data: resp.data.items });
    });
    promises.push(newPromise);
    Promise.all(promises).then(() => {
      this.setState({ flag: true });
    });
  }

  switch = () => {
    this.setState({ displayShortOrFull: !this.state.displayShortOrFull });
  };

  render() {
    return (
      <React.Fragment>
        {this.state.displayShortOrFull ? (
          this.state.flag ? (
            <Followers array={this.state.data} />
          ) : (
            <p>LOADING...</p>
          )
        ) : (
          <UserFollowers id={this.props.id} />
        )}
        <center>
          <a onClick={() => this.switch()} className="text-success center">
            {this.state.displayShortOrFull ? (
              <p>All Followers</p>
            ) : (
              <p>Top Five Followers</p>
            )}
          </a>
        </center>
      </React.Fragment>
    );
  }
}

export default UserFollowersTopFive;

function Followers(props) {
  let array1 = props.array;
  if (array1.length === 0) {
    return (
      <div>
        <center>
          <p>No Followers</p>
        </center>
      </div>
    );
  } else {
    let mapper = array1.map(array => (
      <div key={array.receiverId} className="panel-body body">
        <div className="media">
          <div className="img-circular pull-left">
            <img src={array.avatarUrl} />
          </div>
          <div className="media-body">
            <a href="#" className="text-success">
              {array.firstName} {array.lastName}
            </a>
            <p>{array.type}</p>
            <small className="block">Followers: {array.followers}</small>
          </div>
        </div>
      </div>
    ));
    return <React.Fragment>{mapper}</React.Fragment>;
  }
}
