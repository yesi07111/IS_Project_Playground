import { Activity, ActivityDetail } from "../interfaces/Activity";
import { Resource, ResourceDate } from "../interfaces/Resource";
import { Facility } from "../interfaces/Facility";
import { UserResponse } from "../interfaces/User";

const CACHE_KEY_ACTIVITIES = 'cachedActivities';
const CACHE_KEY_RESOURCES = 'catchedResources';
const CACHE_KEY_ACTIVITY_DETAILS = 'cachedActivityDetails';
const CACHE_KEY_IMAGES = 'cachedActivityImages';
const CACHE_KEY_FACILITY_TYPES = 'cachedFacilityTypes';
const CACHE_KEY_ACTIVITY_TYPES = 'cachedActivityTypes';
const CACHE_KEY_EDUCATORS = 'cachedEducators';
const CACHE_KEY_TOP_ACTIVITIES = 'cachedTopActivities';
const CACHE_KEY_PROFILE_IMAGES = 'cachedProfileImages'
const CACHE_KEY_RESOURCE_TYPES = 'cachedResourceTypes';
const CACHE_KEY_FACILITY_NAMES = 'cachedFacilityNames';
const CACHE_KEY_RESOURCEDATES = 'cachedResourceDates';
const CACHE_KEY_FACILITY_LOCATIONS = 'cachedFacilityLocations';
const CACHE_KEY_FACILITIES = 'cachedFacilities';
const CACHE_KEY_USERS = 'cachedUsers';

export const cacheService = {
  saveActivities: (activities: Activity[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITIES, JSON.stringify(activities));
  },

  loadActivities: () => {
    const cachedActivities = localStorage.getItem(CACHE_KEY_ACTIVITIES);
    return cachedActivities ? JSON.parse(cachedActivities) : [];
  },

  saveResources: (resources: Resource[]) => {
    localStorage.setItem(CACHE_KEY_RESOURCES, JSON.stringify(resources));
  },

  loadResources: () => {
    const cachedResources = localStorage.getItem(CACHE_KEY_RESOURCES);
    return cachedResources ? JSON.parse(cachedResources) : [];
  },

  saveResourceDates: (resources: ResourceDate[]) => {
    localStorage.setItem(CACHE_KEY_RESOURCEDATES, JSON.stringify(resources));
  },

  loadResourceDates: () => {
    const cachedResources = localStorage.getItem(CACHE_KEY_RESOURCEDATES);
    return cachedResources ? JSON.parse(cachedResources) : [];
  },

  saveUsers: (users: UserResponse[]) => {
    localStorage.setItem(CACHE_KEY_USERS, JSON.stringify(users));
  },

  loadUsers: () => {
    const cachedUsers = localStorage.getItem(CACHE_KEY_USERS);
    return cachedUsers ? JSON.parse(cachedUsers) : [];
  },

  saveActivityDetails: (activityDetails: ActivityDetail[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITY_DETAILS, JSON.stringify(activityDetails));
  },

  loadActivityDetails: () => {
    const cachedActivityDetails = localStorage.getItem(CACHE_KEY_ACTIVITY_DETAILS);
    return cachedActivityDetails ? JSON.parse(cachedActivityDetails) : [];
  },

  saveImages: (images: { [id: string]: string }) => {
    localStorage.setItem(CACHE_KEY_IMAGES, JSON.stringify(images));
  },

  loadImages: () => {
    const cachedImages = localStorage.getItem(CACHE_KEY_IMAGES);
    return cachedImages ? JSON.parse(cachedImages) : {};
  },

  saveFacilityTypes: (facilityTypes: string[]) => {
    localStorage.setItem(CACHE_KEY_FACILITY_TYPES, JSON.stringify(facilityTypes));
  },

  loadFacilityTypes: () => {
    const cachedFacilityTypes = localStorage.getItem(CACHE_KEY_FACILITY_TYPES);
    return cachedFacilityTypes ? JSON.parse(cachedFacilityTypes) : [];
  },

  saveFacilityLocations: (facilityLocations: string[]) => {
    localStorage.setItem(CACHE_KEY_FACILITY_LOCATIONS, JSON.stringify(facilityLocations));
  },

  loadFacilityLocations: () => {
    const cachedFacilityLocations = localStorage.getItem(CACHE_KEY_FACILITY_LOCATIONS);
    return cachedFacilityLocations ? JSON.parse(cachedFacilityLocations) : [];
  },

  saveFacilities: (facility: Facility[]) => {
    localStorage.setItem(CACHE_KEY_FACILITIES, JSON.stringify(facility));
  },

  loadFacilities: () => {
    const cachedFacilities = localStorage.getItem(CACHE_KEY_FACILITIES);
    return cachedFacilities ? JSON.parse(cachedFacilities) : [];
  },

  saveFacilityNames: (facilityNames: string[]) => {
    localStorage.setItem(CACHE_KEY_FACILITY_NAMES, JSON.stringify(facilityNames));
  },

  loadFacilityNames: () => {
    const cachedFacilityNames = localStorage.getItem(CACHE_KEY_FACILITY_NAMES);
    return cachedFacilityNames ? JSON.parse(cachedFacilityNames) : [];
  },

  saveActivityTypes: (activityTypes: string[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITY_TYPES, JSON.stringify(activityTypes));
  },

  loadActivityTypes: () => {
    const cachedActivityTypes = localStorage.getItem(CACHE_KEY_ACTIVITY_TYPES);
    return cachedActivityTypes ? JSON.parse(cachedActivityTypes) : [];
  },

  saveResourceTypes: (resourceTypes: string[]) => {
    localStorage.setItem(CACHE_KEY_RESOURCE_TYPES, JSON.stringify(resourceTypes));
  },

  loadResourceTypes: () => {
    const cachedResourceTypes = localStorage.getItem(CACHE_KEY_RESOURCE_TYPES);
    return cachedResourceTypes ? JSON.parse(cachedResourceTypes) : [];
  },

  saveEducators: (educators: { id: string, displayName: string }[]) => {
    localStorage.setItem(CACHE_KEY_EDUCATORS, JSON.stringify(educators));
  },

  loadEducators: () => {
    const cachedEducators = localStorage.getItem(CACHE_KEY_EDUCATORS);
    return cachedEducators ? JSON.parse(cachedEducators) : [];
  },

  saveActivityDetail: (activityDetail: ActivityDetail) => {
    const activityDetails = cacheService.loadActivityDetails();
    const updatedActivityDetails = activityDetails.filter((a: ActivityDetail) => a.id !== activityDetail.id);
    updatedActivityDetails.push(activityDetail);
    cacheService.saveActivityDetails(updatedActivityDetails);
  },

  loadActivityDetail: (id: string) => {
    const activityDetails = cacheService.loadActivityDetails();
    return activityDetails.find((activityDetail: ActivityDetail) => activityDetail.id === id) || null;
  },

  saveTopActivities: (topActivities: Activity[]) => {
    localStorage.setItem(CACHE_KEY_TOP_ACTIVITIES, JSON.stringify(topActivities));
  },

  loadTopActivities: () => {
    const cachedTopActivities = localStorage.getItem(CACHE_KEY_TOP_ACTIVITIES);
    return cachedTopActivities ? JSON.parse(cachedTopActivities) : [];
  },

  saveUserImages: (username: string, mainImage: string, otherImages: string[]) => {
    const cachedImages = cacheService.loadUsersImages();
    cachedImages[username] = { main: mainImage, others: otherImages };
    localStorage.setItem(CACHE_KEY_PROFILE_IMAGES, JSON.stringify(cachedImages));
  },

  loadUsersImages: () => {
    const cachedImages = localStorage.getItem(CACHE_KEY_PROFILE_IMAGES);
    return cachedImages ? JSON.parse(cachedImages) : {};
  },

  loadUserImages: (username: string) => {
    const cachedImages = localStorage.getItem(CACHE_KEY_PROFILE_IMAGES);
    const cachedImagesObject = cachedImages ? JSON.parse(cachedImages) : {};
    return cachedImagesObject[username] || { main: '', others: [] };
  },

  clearCache: () => {
    localStorage.setItem(CACHE_KEY_ACTIVITIES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_ACTIVITY_DETAILS, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_IMAGES, JSON.stringify({}));
    localStorage.setItem(CACHE_KEY_FACILITY_TYPES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_ACTIVITY_TYPES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_EDUCATORS, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_TOP_ACTIVITIES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_PROFILE_IMAGES, JSON.stringify({}));
  }
};